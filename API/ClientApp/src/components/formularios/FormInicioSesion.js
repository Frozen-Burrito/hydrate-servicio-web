import React, { useState } from 'react';
import useCookie from '../../utils/useCookie';
import { Link, useHistory, Redirect } from 'react-router-dom';
import { iniciarSesion } from '../../api/api';
import { 
  validarNombreUsuario, 
  validarCorreo, 
  validarPassword,
  estaVacio,
  ErrorDeAutenticacion,
  ErrorDeValidacion 
} from '../../utils/validaciones';

import './formularios.css';

function FormInicioSesion() {
  
  // Acceder a la cookie con el JWT, para actualizarlo o ver si ya existe.
  const { valor: token, actualizarCookie: setToken } = useCookie('jwt');

  const history = useHistory();

  // Declarar estado con valor y errores para cada campo del formulario. 
  // Ambos comienzan como strings vacíos.
  const [correoOUsuario, setCorreoOUsuario] = useState('');
  const [errCorreoOUsuario, setErrCorreoOUsuario] = useState('');

  const [password, setPassword] = useState('');
  const [errPassword, setErrPassword] = useState('');

  const [errGeneral, setErrGeneral] = useState('');

  // Describe si el formulario esta haciendo una peticion a la API.
  const [estaCargando, setEstaCargando] = useState(false);

  // Recibe el valor del campo de correo o nombre de usuario.
  // Lo valida y actualiza el estado del valor o el error en el formulario.
  const handleCambioCorreoOUsuario = (e) => {
    const valor = e.target.value;
    
    const resultadoVal = (valor.indexOf('@') > -1) 
      ? validarCorreo(valor)
      : validarNombreUsuario(valor);

    setCorreoOUsuario(valor);

    if (resultadoVal === null) {
      setErrCorreoOUsuario('');

    } else {
      if (resultadoVal.error === ErrorDeValidacion.correoVacio.error) {
        setErrCorreoOUsuario('El correo electrónico o nombre de usuario es obligatorio');

      } else if (resultadoVal.error === ErrorDeValidacion.correoNoValido.error) {
        setErrCorreoOUsuario('El correo no tiene un formato válido');
      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioNoValido) {
        setErrCorreoOUsuario('El nombre de usuario no tiene un formato válido');
      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioMuyCorto) {
        setErrCorreoOUsuario('El nombre de usuario debe tener más de 4 caracteres');
      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioNoValido) {
        setErrCorreoOUsuario('El nombre de usuario debe tener menos de 20 caracteres');
      }
    }
  }

  const handleCambioPassword = (e) => {
    
    const passIntroducido = e.target.value;
    
    const resultadoVal = validarPassword(passIntroducido);

    setPassword(passIntroducido);

    if (resultadoVal=== null) {
      setErrPassword('');

    } else {
      if (resultadoVal.error === ErrorDeValidacion.passwordVacio.error) {
        setErrPassword('La contraseña es obligatoria');

      } else if (resultadoVal.error === ErrorDeValidacion.passwordMuyCorto.error) {
        setErrPassword('La contraseña debe tener más de 8 caracteres.');

      } else if (resultadoVal.error === ErrorDeValidacion.passwordMuyLargo.error) {
        setErrPassword('La contraseña debe tener menos de 40 caracteres.');

      } else if (resultadoVal.error === ErrorDeValidacion.passwordNoValido.error) {
        setErrPassword('La contraseña no tiene un formato válido');
      }
    }
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setEstaCargando(true);

    const credenciales = {
      password
    };

    if (validarCorreo(correoOUsuario) === null) credenciales.email = correoOUsuario
    else if (validarNombreUsuario(correoOUsuario) === null) credenciales.nombreUsuario = correoOUsuario;
    
    const resultado = await iniciarSesion(credenciales);

    if (resultado.ok) {
      // La petición de registro fue exitosa, guardar token y 
      // redirigir a inicio.
      setToken(resultado.cuerpo.token);
  
      history.push('/');

    } else if (resultado.status >= 500) {
      setErrGeneral('El servicio no está disponible, intente más tarde');

    } else if (resultado.status >= 400) {

      const tipoError = Object.keys(ErrorDeAutenticacion)[resultado.cuerpo['tipo']];

      if (tipoError === ErrorDeAutenticacion.usuarioNoExiste.error) {
        setErrCorreoOUsuario('No existe una cuenta con este correo o nombre de usuario');

      } else if (tipoError === ErrorDeAutenticacion.passwordIncorrecto.error) {
        setErrPassword('La contraseña es incorrecta');
      } else if (tipoError === ErrorDeAutenticacion.formatoIncorrecto.error) {
        setErrGeneral('Las credenciales no tienen el formato correcto');
      }
    }

    setEstaCargando(false);
  }

  // Es true si existen errores de validación en el formulario.
  const tieneErrores = !estaVacio(errCorreoOUsuario) || !estaVacio(errPassword);

  // Desactivar el botón de enviar formulario si está cargando o tiene errores.
  const submitDesactivado = estaCargando || tieneErrores;

  // Declarar el componente para el formulario.
  const formulario = (
    <div className='form-container'>
      <Link to='/' className='no-underline-link'>
        <h3 className='form-logo'>Hydrate</h3>
      </Link>

      <form action="">
        <h2 className='center-text mt-5'>Iniciar Sesión</h2>

        <div className='form-fields'>
          <div className='form-group'>
            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  email
                </span>
                <input 
                  type='text' 
                  name='correo' 
                  className='input' 
                  placeholder='Correo Electrónico o Usuario'
                  value={correoOUsuario}
                  onChange={handleCambioCorreoOUsuario}
                />
              </div>

              <p className='error' >
                {errCorreoOUsuario}
              </p>
            </div>
          </div>

          <div className='form-group'>
            <div className="campo">
              <div className="campo-con-icono">
                <span className="material-icons">
                  vpn_key
                </span>
                <input 
                  type='password' 
                  name='password' 
                  className='input' 
                  placeholder='Constraseña'
                  value={password}
                  onChange={handleCambioPassword}
                />
              </div>

              <p className='error' >
                {errPassword}
              </p>
            </div>
          </div>

          <p className='text-acount'>
            ¿No tienes una cuenta? 
            <Link className='account-link' to='/creacion-cuenta'>Registrate</Link>
          </p>

          <p className='error' >
            {errGeneral}
          </p>
        </div>

        <div className='center-text mt-1'>
          <button 
            className={`btn btn-primario ${submitDesactivado ? 'btn-desactivado' : ''}`}
            disabled={submitDesactivado} 
            onClick={handleSubmit}
          >
            Iniciar Sesión
          </button>
        </div>
      </form>
    </div>
  );
  
  const usuarioAutenticado = token !== undefined && token !== null;

  // Si el usuario no está autenticado,  mostrar el formulario.
  // Si ya lo está, redirigir a "/".
  return !usuarioAutenticado 
    ? <>{formulario}</>
    : <Redirect
        replace={true}
        to='/'
      />
}

export default FormInicioSesion;