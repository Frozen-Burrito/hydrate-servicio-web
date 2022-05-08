import React, { useState, useEffect } from "react";
import useCookie from "../../utils/useCookie";
import { agregarRecurso, editarRecurso } from "../../api/api";
import { 
  estaVacio,
  validarTituloRecurso,
  validarUrl,
  validarFechaPub,
  validarDescripcionRecurso,
  ErrorDeRecurso,
} from "../../utils/validaciones";

export const FormAgregarRecurso = ({ recursoActual, onRecursoModificado }) => {

  const { valor: jwt } = useCookie('jwt');

  const recursoExistente = recursoActual !== undefined && recursoActual !== null;

  const [titulo, setTitulo] = useState('');
  const [errTitulo, setErrTitulo] = useState('');

  const [fecha, setFecha] = useState('');
  const [errFecha, setErrFecha] = useState('');

  const [url, setUrl] = useState('');
  const [errUrl, setErrUrl] = useState('');

  const [descripcion, setDescripcion] = useState('');
  const [errDescripcion, setErrDescripcion] = useState('');

  const [errGeneral, setErrGeneral] = useState('');
  const [txtBotonSubmit, setTxtBotonSubmit] = useState('Agregr Recurso');

  // Describe si el formulario esta haciendo una peticion a la API.
  const [estaCargando, setEstaCargando] = useState(false);

  useEffect(() => {
    setTitulo(recursoExistente ? recursoActual.titulo : '');
    setFecha(recursoExistente ? recursoActual.fechaPublicacion.substring(0, 10) : '');
    setUrl(recursoExistente ? recursoActual.url : '');
    setDescripcion(recursoExistente ? recursoActual.descripcion : '');

    setTxtBotonSubmit(recursoExistente ? 'Confirmar Cambios' : 'Agregr Recurso');
    
  }, [recursoActual])
  

  const handleCambioTitulo = (e) => {
    const tituloIntroducido = e.target.value;

    setTitulo(tituloIntroducido);
    
    const resultadoVal = validarTituloRecurso(tituloIntroducido);

    if (resultadoVal === null) {
      setErrTitulo('');

    } else {
      if (resultadoVal.error === ErrorDeRecurso.errTituloVacio.error) {
        setErrTitulo('El título del recurso es obligatorio');

      } else if (resultadoVal.error === ErrorDeRecurso.errTituloMuyLargo.error) {
        setErrTitulo('El título del recurso debe tener menos de 40 caracteres');
      } 
    }
  }

  const handleCambioURL = (e) => {
    const urlIntroducida = e.target.value;

    setUrl(urlIntroducida);

    const resultadoVal = validarUrl(urlIntroducida);

    if (resultadoVal === null) {
      setErrUrl('');

    } else {
      if (resultadoVal.error === ErrorDeRecurso.errUrlVacia.error) {
        setErrUrl('La URL del recurso informativo es obligatoria');

      } else if (resultadoVal.error === ErrorDeRecurso.errUrlSinHttps.error) {
        setErrUrl('La URL del recurso informativo debe usar HTTPS');
      } 
    }
  }

  const handleCambioDescripcion = (e) => {
    const descripcionIntroducida = e.target.value;

    setDescripcion(descripcionIntroducida);

    const resultadoVal = validarDescripcionRecurso(descripcionIntroducida);

    if (resultadoVal === null) {
      setErrDescripcion('');

    } else {
      if (resultadoVal.error === ErrorDeRecurso.errDescripcionMuyLarga.error) {
        setErrDescripcion('La descripción del recurso debe tener menos de 500 caracteres');

      }
    }
  }

  const handleCambioFecha = (e) => {
    const fechaIntroducida = e.target.valueAsDate;

    setFecha(fechaIntroducida.toISOString().substring(0, 10));

    const resultadoVal = validarFechaPub(fechaIntroducida);

    if (resultadoVal === null) {
      setErrFecha('');

    } else {
      if (resultadoVal.error === ErrorDeRecurso.errFechaNoValida.error) {
        setErrFecha('La fecha de publicación del recurso debe ser anterior a la fecha actual.');
      }
    }
  }

  const handleSubmit = async (e) => {
    // Evitar el comportamiento por defecto de submit, donde se 
    // refresca la pagina.
    e.preventDefault();
    setEstaCargando(true);

    const statusEsperado = recursoExistente ? 204 : 201;

    let resultado = null;

    if (recursoExistente) {
      // Crear objeto de recurso modificado, para que sea serializado a JSON por 
      // editarRecurso.
      const recursoModificado = {
        id: recursoActual.id,
        titulo,
        url,
        descripcion,
        fechaPublicacion: fecha,
      };

      resultado = await editarRecurso(jwt, recursoModificado);

      resultado.cuerpo = recursoModificado;
    } else {
      const recursoCreado = {
        titulo,
        url,
        descripcion,
        fechaPublicacion: fecha,
      };

      resultado = await agregarRecurso(jwt, recursoCreado);
    }   

    if (resultado !== null) {
      if (resultado.ok && resultado.status === statusEsperado) {
        // La petición de creacion del recurso fue exitosa.
        console.log(resultado.cuerpo);
  
        setTitulo('');
        setUrl('');
        setFecha('');
        setDescripcion('');
        
        // Manejar una modificación en el recurso (agregado o editado).
        onRecursoModificado(resultado.cuerpo);
  
      } else if (resultado.status >= 500) {
        setErrGeneral('El servicio no está disponible, intente más tarde.');
  
      } else if (resultado.status >= 400) {
  
        console.log(resultado.cuerpo);
      }
    } else {
      setErrGeneral('El servicio no está disponible, intente más tarde.');
    }

    setEstaCargando(false);
  }

  // Es true si existen errores de validación en el formulario.
  const tieneErrores = !estaVacio(errTitulo) || !estaVacio(errUrl) 
  || !estaVacio(errFecha) || !estaVacio(errDescripcion);

  // Evitar que el usuario deje vacios campos, si no los modifica y nunca se 
  // ejecutan las validaciones (el usuario tiene que escribir algo para que se valide).
  const noTieneValores = estaVacio(titulo) || estaVacio(url) || estaVacio(fecha);
    
  // Desactivar el botón de enviar formulario si está cargando o tiene errores.
  const submitDesactivado = estaCargando || tieneErrores || noTieneValores;

  return (
    <form>
        <div className='form-fields'>
          <div className='form-group'>
            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  auto_stories
                </span>
                <input 
                  type='text' 
                  name='titulo' 
                  required
                  disabled={estaCargando}
                  className='input' 
                  placeholder='Título del recurso'
                  value={titulo}
                  onChange={e => handleCambioTitulo(e)}/>
              </div>

              <p className='error' >
                {errTitulo}
              </p>
            </div>

            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  event
                </span>
                <input 
                  type='date' 
                  name='fechaPub' 
                  required
                  disabled={estaCargando}
                  className='input' 
                  placeholder='Fecha de Publicación' 
                  value={fecha}
                  onChange={e => handleCambioFecha(e)}/>
              </div>

              <p className='error' >
                {errFecha}
              </p>
            </div>
          </div>

          <div className='form-group'>
            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  link
                </span>
                <input 
                  type='text' 
                  name='url' 
                  required
                  disabled={estaCargando}
                  className='input' 
                  placeholder='URL' 
                  value={url}
                  onChange={e => handleCambioURL(e)}/>
              </div>

              <p className='error' >
                {errUrl}
              </p>
            </div>
          </div>

          <div className='form-group'>
            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  subject
                </span>
                <input 
                  type='text' 
                  name='descripcion'
                  required 
                  disabled={estaCargando}
                  className='input' 
                  placeholder='Descripción breve' 
                  value={descripcion}
                  onChange={e => handleCambioDescripcion(e)}/>
              </div>
              <p className='error' >
                {errDescripcion}
              </p>
            </div>
          </div>

          <p className='error' >
            {errGeneral}
          </p>
        </div>

        <div className='mt-1'>
          <button 
            className={`btn btn-primario ${submitDesactivado ? 'btn-desactivado' : ''}`}
            disabled={submitDesactivado} 
            onClick={handleSubmit}
          >
            { txtBotonSubmit }
          </button>
        </div>
      </form>
  );
}