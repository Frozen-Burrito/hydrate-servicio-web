import React, { useState, useEffect } from "react";
import { Link, useHistory } from "react-router-dom";

import { 
    estaVacio, 
    validarCorreo,
    validarAsuntoComentario,
    validarContenidoComentario,
    ErrorDeValidacion,
    ErrorDeComentario
} from "../../utils/validaciones";
import useCookie from "../../utils/useCookie"; 

import { 
    StatusHttp,
    resultadoEsOK,
    resultadoEsErrCliente,
    resultadoEsErrServidor 
} from "../../api/api";
import { 
    publicarComentario, 
    fetchComentarioConId,
    modificarComentario,
    publicarRespuestaAComentario
} from "../../api/api_comentarios";
import { registrarUsuarioTemporal } from "../../api/api_auth";

import './formularios.css';

FormPublicarComentario.defaultProps = {
    idComentario: -1,
    esRespuesta: false,
};

export default function FormPublicarComentario(props) {

    const { esRespuesta, idComentario } = props;

    const idExistente = idComentario != null && idComentario >= 0;
    const editando = idExistente && !esRespuesta;

    const { valor: jwt } = useCookie('jwt');

    const textoEncabezado = idExistente 
        ? (esRespuesta ? "Publica una Respuesta" : "Modifica tu Comentario" )
        : "Envía un Comentario";

    const [comentario, setComentario] = useState(null);

    const [valores, setValores] = useState({
        email: "",
        asunto: "",
        contenido: "",
    });

    const [errores, setErrores] = useState({
        general: "",
        email: "",
        asunto: "",
        contenido: "",
    });

    const [longitudesMax] = useState({
        asunto: 100,
        contenido: 500
    });

    const [estaCargando, setEstaCargando] = useState(false);

    const history = useHistory();

    /**
     * Valida y actualiza el estado de los valores y errores del
     * formulario.
     */
    const handleCambioValor = (e) => {

        const nombreValor = e.target.name;
        const valor = e.target.value;

        let resultadoVal = "";

        switch (nombreValor) {
            case "email": 
                resultadoVal = validarCorreo(valor);
                break;
            case "asunto": 
                resultadoVal = validarAsuntoComentario(valor, longitudesMax.asunto);
                break;
            case "contenido": 
                resultadoVal = validarContenidoComentario(valor, longitudesMax.contenido);
                break;
            default:
                console.error(`Un valor que no existe fue modificado: [${nombreValor}]: ${valor}`);
                break;
        }

        setValores({
            ...valores,
            [nombreValor]: valor, 
        });

        setComentario({
            ...comentario,
            [nombreValor]: valor, 
        });

        let errorEnValor = "";

        if (resultadoVal != null) {

            if (resultadoVal.error === ErrorDeValidacion.correoVacio.error) {
                errorEnValor = "El correo electrónico es obligatorio";
        
            } else if (resultadoVal.error === ErrorDeValidacion.correoNoValido.error) {
                errorEnValor = "El correo no tiene un formato válido";
            } else if (resultadoVal.error === ErrorDeComentario.errAsuntoVacio.error) {
                errorEnValor = "El asunto es obligatorio";
            } else if (resultadoVal.error === ErrorDeComentario.errAsuntoMuyLargo.error) {
                errorEnValor = `El asunto debe tener menos de ${longitudesMax.asunto} caracteres`;
            } else if (resultadoVal.error === ErrorDeComentario.errContenidoVacio.error) {
                errorEnValor = "El contenido es obligatorio";
            } else if (resultadoVal.error === ErrorDeComentario.errContenidoMuyLargo.error) {
                errorEnValor = `El contenido debe tener menos de ${longitudesMax.contenido} caracteres`;
            } 
        }

        setErrores({
            ...errores,
            [nombreValor]: errorEnValor,
        });
    }

    const handlePublicarOEditarComentario = async (e) => {
        e.preventDefault();
        setEstaCargando(true);

        const { asunto, contenido } = valores;

        const nuevoComentario = {
            asunto, contenido,
        };

        let usandoCuentaValida = jwt != null;

        if (!usandoCuentaValida) {

            console.log("Cuenta no existe, registrando usuario temporal.");

            const respuestaRegistro = await registrarUsuarioTemporal(valores.email);

            console.log(respuestaRegistro);

            usandoCuentaValida = respuestaRegistro.ok && 
                                    respuestaRegistro.status === 200;

            console.log(usandoCuentaValida);
        }

        if (usandoCuentaValida) {

            const resultado = editando 
                ? await modificarComentario(comentario, jwt)
                : await publicarComentario(nuevoComentario, jwt);
    
            if (resultado != null) {

                const { ok, status, cuerpo } = resultado;

                if (ok && resultadoEsOK(status)) {
                    console.log(cuerpo);

                    history.push("/comentarios");

                } else if (resultadoEsErrCliente(status)) {
                    setEstaCargando(false);
                    setErrores({ 
                        ...errores, 
                        general: "El servicio no está disponible, intente más tarde."
                    });

                } else if (resultadoEsErrServidor(status)) {
                    setEstaCargando(false);
                    setErrores({ 
                        ...errores, 
                        general: resultado.cuerpo.mensaje
                    });
                }
            } else {
                setEstaCargando(false);
                setErrores({ 
                    ...errores, 
                    general: "Hubo un error inesperado."
                });
            }
        } else {
            setEstaCargando(false);
            setErrores({ 
                ...errores, 
                general: "No fue posible autenticarlo, el comentario no puede ser publicado."
            });
        }
    }

    const handlePublicarRespuesta = async (e) => {
        e.preventDefault();

        if (!esRespuesta) return;

        setEstaCargando(true);

        const respuesta = {
            contenido: valores.contenido
        };

        let usandoCuentaValida = jwt != null;

        if (!usandoCuentaValida) {

            const respuestaRegistro = await registrarUsuarioTemporal(valores.email);

            usandoCuentaValida = respuestaRegistro.ok && 
                                    respuestaRegistro.status === StatusHttp.Status200OK;
        }

        if (usandoCuentaValida) {

            const resultado = await publicarRespuestaAComentario(respuesta, idComentario, jwt);
    
            if (resultado != null) {

                const { ok, status, cuerpo } = resultado;

                if (ok && resultadoEsOK(status)) {
                    console.log(cuerpo);

                    history.push("/comentarios");

                } else if (resultadoEsErrCliente(status)) {
                    setEstaCargando(false);
                    setErrores({ 
                        ...errores, 
                        general: "El servicio no está disponible, intente más tarde."
                    });

                } else if (resultadoEsErrServidor(status)) {
                    setEstaCargando(false);
                    setErrores({ 
                        ...errores, 
                        general: resultado.cuerpo.mensaje
                    });
                }
            } else {
                setEstaCargando(false);
                setErrores({ 
                    ...errores, 
                    general: "Hubo un error inesperado."
                });
            }
        } else {
            setEstaCargando(false);
            setErrores({ 
                ...errores, 
                general: "No fue posible autenticarlo, el comentario no puede ser publicado."
            });
        }
    }

    useEffect(() => {
        async function obtenerComentario() {
            setEstaCargando(true);

            // Obtener el comentario segun el ID recibido.
            const resultado = await fetchComentarioConId(idComentario, jwt);
      
            if (resultado.ok && resultado.status === StatusHttp.Status200OK) {
                
                const comentario = resultado.cuerpo;

                setComentario(comentario);
                
                if (editando) {
                    setValores({
                        ...valores,
                        asunto: comentario.asunto,
                        contenido: comentario.contenido,
                    });
                }

                setEstaCargando(false);

            } else {
              setErrores({
                  ...errores,
                  general: "No fue posible acceder al comentario referenciado.",
              });

              setEstaCargando(false);
            }
        }

        if (comentario == null && idExistente) {
            obtenerComentario();
        }

    }, [ idComentario, jwt, valores, errores, comentario, idExistente, editando ]);
    
    const renderCampoAsunto = () => {
        if (esRespuesta) { 
            return null;
        } else {
            return (
                <div className="form-group">
                    <div className="campo expandir">
                        <div className="campo-con-icono">
                            <span className="material-icons">
                                format_quote
                            </span>
                            <input 
                                required
                                type='text' 
                                name='asunto' 
                                disabled={estaCargando}
                                className='input' 
                                placeholder='Tema o asunto'
                                value={valores.asunto}
                                onChange={handleCambioValor}/>
                        </div>

                        <div className="stack horizontal justify-between gap-2 mt-1">
                            <p className="error">
                                {errores.asunto}
                            </p>

                            <p className={longitudAsuntoExcede ? "error" : ""}>
                                {`${valores.asunto.length} / ${longitudesMax.asunto}`}
                            </p>
                        </div>
                    </div>
                </div>
            );
        }
    }

    const longitudAsuntoExcede = valores.asunto.length > longitudesMax.asunto;
    const longitudContenidoExcede = valores.contenido.length > longitudesMax.contenido;

    // Es true si existen errores de validación en el formulario.
    // (Todos los campos son obligatorios).
    const tieneErrores = !estaVacio(errores.email) || !estaVacio(errores.asunto) 
    || !estaVacio(errores.contenido);
    
    // Desactivar el botón de enviar formulario si está cargando o tiene errores.
    const submitDesactivado = estaCargando || tieneErrores;

    return (
        <div className='form-container w-50'>
            <form>
                <h2 className='center-text mt-7 mb-3'>{textoEncabezado}</h2>

                <div className='form-fields'>
                    <div className='form-group'>
                        <div className="campo expandir">
                            <div className="campo-con-icono">
                                <span className="material-icons">
                                    email
                                </span>
                                <input 
                                    type='email' 
                                    name='email' 
                                    required
                                    disabled={estaCargando}
                                    className='input' 
                                    placeholder='Correo Electrónico'
                                    value={valores.email}
                                    onChange={handleCambioValor}/>
                            </div>

                            <div className="stack horizontal justify-between gap-2 mt-1">
                                <p className='error' >
                                    {errores.email}
                                </p>
                            </div>
                        </div>
                    </div>

                    { renderCampoAsunto() }

                    <div className="form-group">
                        <div className="campo expandir">
                            <div className="campo-con-icono">
                                <span className="material-icons">
                                    subject
                                </span>

                                <textarea 
                                    name="contenido"
                                    placeholder={esRespuesta ? "¿Cuál es tu respuesta al comentario?" : "Describe tu situación..."}
                                    className="input"
                                    required
                                    disabled={estaCargando}
                                    value={valores.contenido}
                                    onChange={handleCambioValor}
                                />
                            </div>

                            <div className="stack horizontal justify-between gap-2 mt-1">
                                <p className='error' >
                                    {errores.contenido}
                                </p>

                                <p className={longitudContenidoExcede ? "error" : ""}>
                                    {`${valores.contenido.length} / ${longitudesMax.contenido}`}
                                </p>
                            </div>
                        </div>
                    </div>

                    <p className='text-acount'>
                        ¿No tienes una cuenta? 
                        <Link className='account-link' to='/creacion-cuenta'>Registrate</Link>
                    </p>

                    <p className='error' >
                        {errores.general}
                    </p>
                </div>

                <div className='end-text mt-1'>
                    <button 
                        className={`btn btn-primario ${submitDesactivado ? 'btn-desactivado' : ''}`}
                        disabled={submitDesactivado} 
                        onClick={esRespuesta ? handlePublicarRespuesta : handlePublicarOEditarComentario}
                    >
                        Publicar
                    </button>
                </div>
            </form>          
        </div>
    );
}