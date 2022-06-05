import React, { useState } from "react";
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
    resultadoEsOK,
    resultadoEsErrCliente,
    resultadoEsErrServidor 
} from "../../api/api";
import { publicarComentario } from "../../api/api_comentarios";
import { registrarUsuarioTemporal } from "../../api/api_auth";

import './formularios.css';

FormPublicarComentario.defaultProps = {
    esRespuesta: false,
};

export default function FormPublicarComentario(props) {

    const { esRespuesta } = props;

    const { valor: token, eliminarCookie: eliminarToken } = useCookie('jwt');

    const textoEncabezado = esRespuesta ? "Responder a Comentario" : "Enviar un Comentario";

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

    const [longitudesMax, _] = useState({
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
        }

        setValores({
            ...valores,
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

    const handlePublicarComentario = async (e) => {
        e.preventDefault();
        setEstaCargando(true);

        const { asunto, contenido } = valores;

        const nuevoComentario = {
            asunto, contenido
        };

        let usandoCuentaValida = token != null;

        if (!usandoCuentaValida) {

            const respuestaRegistro = await registrarUsuarioTemporal(valores.email);

            usandoCuentaValida = respuestaRegistro.ok && 
                                    respuestaRegistro.status === 200;
        }

        if (usandoCuentaValida) {
            const resultado = await publicarComentario(nuevoComentario, token);
    
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
                        <div className="campo">
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

                    <div className="form-group">
                        <div className="campo">
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

                    <div className="form-group">
                        <div className="campo">
                            <div className="campo-con-icono">
                                <span className="material-icons">
                                    subject
                                </span>

                                <textarea 
                                    name="contenido"
                                    placeholder="Describe tu situación..."
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
                        onClick={handlePublicarComentario}
                    >
                        Publicar
                    </button>
                </div>
            </form>          
        </div>
    );
}