import './Navbar.css';
import React from 'react'
import { Link } from 'react-router-dom';
import useCookie from '../../utils/useCookie';
import Dropdown from '../Dropdown/dropdown';
import BotonRedondeado from '../Botones/BotonRedondeado';

export default function Navbar() {

  const { valor: token, eliminarCookie: eliminarToken } = useCookie('jwt');

  const textoRegistro = 'Regístrate';
  const textoIniciarSesion = 'Iniciar Sesión';
  const urlIniciarSesion = '/inicio-sesion';
  const urlRegistro = '/creacion-cuenta';

  const botonesRegistro = (
    <>
      <BotonRedondeado 
        relleno={true} 
        texto={textoIniciarSesion} 
        link={urlIniciarSesion} 
      />
      <BotonRedondeado 
        relleno={false} 
        texto={textoRegistro} 
        link={urlRegistro} 
      />
    </>
  );

  const btnDropdown = (
    <Dropdown 
      onColor='primary'
      boton={(
        <span className="material-icons">
          account_circle
        </span>
      )}
      items={(
        <>
          <Link to='/perfil' className='elemento-dropdown'>
            Perfil
          </Link>
          <button className ='elemento-dropdown' onClick={() => {
            
            eliminarToken();
          }}>
            Cerrar Sesión
          </button>
        </>
      )}
    />
  );

  return (
    <nav className={`navbar`}>
      <div className='container'>
        <div className='logo-container'>
          <Link exact className='logo' to='/' style={{ textDecoration: 'none' }}>Hydrate</Link>
        </div>

        <div className='links-container'>
          <div className='nav-links'>
            <ul>
              <li className='link'>
                <Link className='nav-link' to='/about' style={{ textDecoration: 'none' }}>Sobre Nosotros</Link>
              </li>
              <li className='link'>
                <Link className='nav-link' to='/productos' style={{ textDecoration: 'none' }}>Productos</Link>
              </li>
              <li className='link'>
                <Link className='nav-link' to='/guias-usuario' style={{ textDecoration: 'none' }}>Guías de Usuario</Link>
              </li>
              <li className='link'>
                <Link className='nav-link' to='/datos-abiertos' style={{ textDecoration: 'none' }}>Datos Abiertos</Link>
              </li>
            </ul>
          </div>
          
          <div className='btns-container'>
            { token === undefined || token === null ? botonesRegistro : btnDropdown }
          </div>
        </div>
      </div>
    </nav>
  );
}
