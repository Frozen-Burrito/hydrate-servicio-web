import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import { useHistory, Redirect } from 'react-router-dom';
import useCookie from '../../utils/useCookie';
import { registrarUsuario } from '../../api/api';
import { 
  validarCorreo, 
  validarNombreUsuario,
  validarPassword,
  validarConfPassword,
  estaVacio, 
  ErrorDeValidacion, 
  ErrorDeAutenticacion
} from '../../utils/validaciones';

function FormCrearCuenta() {

  // Acceder a la cookie con el JWT, para actualizarlo o ver si ya existe.
  const { valor: token, actualizarCookie: setToken } = useCookie('jwt');

  const history = useHistory();

  // Declarar estado con valor y errores para cada campo del formulario. 
  // Ambos comienzan como strings vacíos.
  const [correo, setCorreo] = useState('');
  const [errCorreo, setErrCorreo] = useState('');
  
  const [nombreUsuario, setNombreUsuario] = useState('');
  const [errUsuario, setErrUsuario] = useState('');
  
  const [password, setPassword] = useState('');
  const [errPassword, setErrPassword] = useState('');
  
  const [passConfirm, setPassConfirm] = useState('');
  const [errPassConfirm, setErrPassConfirm] = useState('');

  const [errGeneral, setErrGeneral] = useState('');
  
  // Describe si el formulario esta haciendo una peticion a la API.
  const [estaCargando, setEstaCargando] = useState(false);

  // Recibe el valor del campo de correo.
  // Lo valida y actualiza el estado del valor o el error en el formulario.
  const handleCambioCorreo = (e) => {
    
    const correoIntroducido = e.target.value;
    
    const resultadoVal = validarCorreo(correoIntroducido);

    setCorreo(e.target.value);

    if (resultadoVal=== null) {
      setErrCorreo('');

    } else {
      if (resultadoVal.error === ErrorDeValidacion.correoVacio.error) {
        setErrCorreo('El correo electrónico es obligatorio');

      } else if (resultadoVal.error === ErrorDeValidacion.correoNoValido.error) {
        setErrCorreo('El correo no tiene un formato válido');
      }
    }
  }

  const handleUsernameChange = (e) => {
    
    const nombreIntroducido = e.target.value;
    
    const resultadoVal = validarNombreUsuario(nombreIntroducido);

    setNombreUsuario(nombreIntroducido);

    if (resultadoVal=== null) {
      setErrUsuario('');

    } else {
      if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioVacio.error) {
        setErrUsuario('El nombre de usuario es obligatorio');

      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioMuyCorto.error) {
        setErrUsuario('El nombre de usuario debe tener más de 4 caracteres.');
      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioMuyLargo.error) {
        setErrUsuario('El nombre de usuario debe tener menos de 20 caracteres.');
      } else if (resultadoVal.error === ErrorDeValidacion.nombreUsuarioNoValido.error) {
        setErrUsuario('El nombre de usuario no tiene un formato válido');
      }
    }
  }

  const handlePasswordChange = (e) => {
    
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

  const handlePassConfirmChange = (e) => {

    const confirmacionPassword = e.target.value;
    
    const resultadoVal = validarConfPassword(password, confirmacionPassword);

    setPassConfirm(confirmacionPassword);

    if (resultadoVal=== null) {
      setErrPassConfirm('');

    } else {
      if (resultadoVal.error === ErrorDeValidacion.passConfNoCoincide.error) {
        setErrPassConfirm('Las contraseñas no coinciden.');
      } 
    }
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setEstaCargando(true);

    const credenciales = {
      email: correo,
      nombreUsuario,
      password
    };
    
    const resultado = await registrarUsuario(credenciales);

    if (resultado.ok) {
      // La petición de registro fue exitosa, guardar token y 
      // redirigir a inicio.
      //TODO: Guardar token en una cookie HTTP-only del navegador.
      console.log(resultado.cuerpo);

      setToken(resultado.cuerpo.token);
  
      history.push('/');

    } else if (resultado.status >= 500) {
      setErrGeneral('El servicio no está disponible, intente más tarde.');

    } else if (resultado.status >= 400) {

      console.log(resultado.cuerpo);
      const tipoError = Object.keys(ErrorDeAutenticacion)[resultado.cuerpo['tipo']];

      if (tipoError === ErrorDeAutenticacion.usuarioExiste.error) {
        setErrGeneral('Ya existe un usuario con este correo o nombre de usuario.');

      } else if (tipoError === ErrorDeAutenticacion.formatoIncorrecto.error) {
        setErrGeneral('Las credenciales no tienen el formato correcto.');
      }

      console.log(tipoError.toString());
    }

    setEstaCargando(false);
  }
    
  // Es true si existen errores de validación en el formulario.
  const tieneErrores = !estaVacio(errCorreo) || !estaVacio(errUsuario) 
    || !estaVacio(errPassword) || !estaVacio(errPassConfirm);
    
  // Desactivar el botón de enviar formulario si está cargando o tiene errores.
  const submitDesactivado = estaCargando || tieneErrores;

  // Declarar el componente para el formulario.
  const formulario = (
    <div className='form-container'>
      <Link to='/' className='no-underline-link'>
        <h3 className='form-logo'>Hydrate</h3>
      </Link>

      <form>
        <h2 className='center-text mt-3'>Crear Cuenta</h2>

        <div className='form-fields'>
          <div className='form-group'>
            <div className="campo-con-icono">
              <span className="material-icons">
                email
              </span>
              <input 
                type='text' 
                name='correo' 
                required
                disabled={estaCargando}
                className='input' 
                placeholder='Correo electrónico'
                value={correo}
                onChange={e => handleCambioCorreo(e)}/>
            </div>

            <p className='error' >
              {errCorreo}
            </p>
          </div>

          <div className='form-group'>
            <div className="campo-con-icono">
              <span className="material-icons">
                person
              </span>
              <input 
                type='text' 
                name='nombreUsuario' 
                required
                disabled={estaCargando}
                className='input' 
                placeholder='Nombre de usuario' value={nombreUsuario}
                onChange={e => handleUsernameChange(e)}/>
            </div>

            <p className='error' >
              {errUsuario}
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
                required
                disabled={estaCargando}
                className='input' 
                placeholder='Constraseña' 
                value={password}
                onChange={e => handlePasswordChange(e)}/>
            </div>

            <p className='error' >
              {errPassword}
            </p>
          </div>

          <div className='form-group'>
            <div className="campo-con-icono">
              <span className="material-icons">
                vpn_key
              </span>
              <input 
                type='password' 
                name='passwordConfirm'
                required 
                disabled={estaCargando}
                className='input' 
                placeholder='Confirma tu Constraseña' 
                value={passConfirm}
                onChange={e => handlePassConfirmChange(e)}/>
            </div>
            <p className='error' >
              {errPassConfirm}
            </p>
          </div>

          <p className='text-acount'>
            ¿Ya tienes una cuenta? 
            <Link className='account-link' to='/inicio-sesion'>Iniciar Sesión</Link>
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
            Crear Cuenta
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

export default FormCrearCuenta;