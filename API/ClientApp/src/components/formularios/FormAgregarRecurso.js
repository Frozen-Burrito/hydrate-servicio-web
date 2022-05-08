import React from "react";

export const FormAgregarRecurso = () => {


  const handleCambioTitulo = (e) => {
    
  }

  const handleCambioURL = (e) => {

  }

  const handleCambioDescripcion = (e) => {

  }

  const handleCambioFecha = (e) => {

  }

  return (
    <form>
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
  );
}