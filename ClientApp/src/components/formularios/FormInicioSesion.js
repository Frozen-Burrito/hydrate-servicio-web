import React from 'react';
import { Link } from 'react-router-dom';
import BotonesGenerales from '../../components/Botones/BotonesGenerales';

import './formularios.css';

function FormInicioSesion() {

  return (
    <div className='form-container'>
      <Link to='/' className='no-underline-link'>
        <h3 className='form-logo'>Hydrate</h3>
      </Link>

      <form action="">
        <h2 className='center-text mt-5'>Iniciar Sesión</h2>

        <div className='form-fields'>
          <div className='form-group'>
            <span class="material-icons">
              email
            </span>
            <input type='text' name='correo' className='input' placeholder='Correo Electrónico o Usuario'/>
          </div>

          <div className='form-group'>
            <span class="material-icons">
              vpn_key
            </span>
            <input type='password' name='password' className='input' placeholder='Constraseña'/>
          </div>

          <p className='text-acount'>
            ¿No tienes una cuenta? 
            <Link className='account-link' to='/creacion-cuenta'>Registrate</Link>
          </p>
        </div>

        <div className='center-text mt-5'>
            <BotonesGenerales texto='Iniciar Sesión'/>
        </div>
      </form>
    </div>
  );
}

export default FormInicioSesion;