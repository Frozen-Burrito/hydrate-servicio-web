import './CreacionCuenta.css';
import React from 'react';
import { Link } from 'react-router-dom';
import BotonesGenerales from '../../Components/Botones/BotonesGenerales';

const texto = 'Crear cuenta';

const CreacionCuenta = () => {
  return (
    <main>
      <div className='box-form'>
        <div className='form-container-signup'>
          <form className='form-signup'>
            <h3 className='title'>Hydrate</h3>
            <div className='form'>
              <div className='form-title-container'>
                <h2 className='form-title'>Creación de cuenta</h2>
              </div>
              <div className='input-container'>
                <input type='text' name='name' className='input' />
                <label for=''>Nombre de Usuario</label>
                <span>Nombre de Usuario</span>
              </div>
              <div className='input-container'>
                <input type='text' name='name' className='input' />
                <label for=''>Correo Electrónico</label>
                <span>Correo Electrónico</span>
              </div>
              <div className='input-container'>
                <input type='password' name='name' className='input' />
                <label for=''>Constraseña</label>
                <span>Constraseña</span>
              </div>
              <div className='input-container'>
                <input type='password' name='name' className='input' />
                <label for=''>Confirmar Constraseña</label>
                <span>Constraseña</span>
              </div>
              <p className='text-acount'>¿Ya tienes una cuenta? <Link className='account-link' to='/inicio-sesion' >Inicia sesión</Link></p>
              <p className='text-acount'>Regresar a <Link className='account-link' to='/' >Página de inicio</Link></p>
              <div className='btn-form-container'>
                <BotonesGenerales texto={texto}/>
              </div>
            </div>
          </form>
        </div>
        <div className='img-container-signup'>
        
        </div>
      </div>
    </main>
  );
}

export default CreacionCuenta;