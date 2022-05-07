import React, { useState } from 'react';
import { useHistory } from 'react-router-dom';
import useCookie from '../../utils/useCookie';
import { Link } from 'react-router-dom';
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

  const [ token, setToken ] = useCookie('jwt');

  const history = useHistory();

  const [correoOUsuario, setCorreoOUsuario] = useState('');
  const [errCorreoOUsuario, setErrCorreoOUsuario] = useState('');

  const [password, setPassword] = useState('');
  const [errPassword, setErrPassword] = useState('');

  const [errGeneral, setErrGeneral] = useState('');

  const [estaCargando, setEstaCargando] = useState(false);

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

  const tieneErrores = !estaVacio(errCorreoOUsuario) || !estaVacio(errPassword);

  const submitDesactivado = estaCargando || tieneErrores;

  return (
    <div className='form-container'>
      <Link to='/' className='no-underline-link'>
        <h3 className='form-logo'>Hydrate</h3>
      </Link>

      <form action="">
        <h2 className='center-text mt-5'>Iniciar Sesión</h2>

        <div className='form-fields'>
          <div className='form-group'>
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

          <div className='form-group'>
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
}

export default FormInicioSesion;