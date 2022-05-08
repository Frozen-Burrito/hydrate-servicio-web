import './Navbar.css';
import React from 'react'
import { Link } from 'react-router-dom';
import useCookie from '../../utils/useCookie';
import { obtenerClaims, parseJwt } from '../../utils/parseJwt';
import Dropdown from '../Dropdown/dropdown';
import BotonRedondeado from '../Botones/BotonRedondeado';

export default function Navbar() {

  const { valor: token, eliminarCookie: eliminarToken } = useCookie('jwt');

  var rolDeUsuario = 'NINGUNO';

  if (token !== undefined && token !== null) {
    const datosToken = parseJwt(token);

    const claimsUsuario = obtenerClaims(datosToken);

    rolDeUsuario = claimsUsuario.rol;

    console.log(claimsUsuario);
  }

  const textoRegistro = 'Regístrate';
  const textoIniciarSesion = 'Iniciar Sesión';
  const urlIniciarSesion = '/inicio-sesion';
  const urlRegistro = '/creacion-cuenta';

  const botonesRegistro = (
    <>
      <BotonRedondeado 
        relleno={false} 
        texto={textoIniciarSesion} 
        link={urlIniciarSesion} 
      />
      <BotonRedondeado 
        relleno={true} 
        texto={textoRegistro} 
        link={urlRegistro} 
      />
      <button className='btn-menu-mobile'>
        <span className="material-icons">
          menu
        </span>
      </button>
    </>
  );

  const obtenerOpcionesSegunRol = (rolDeusuario) => {
    if (rolDeUsuario === 'NINGUNO') {
      return (
        <>
          <Link to='/llaves-api' className='elemento-dropdown'>
            Llaves de API
          </Link>
        </>
      );
    } else if (rolDeUsuario === 'MODERADOR_COMENTARIOS') {
      return (
        <>
          <Link to='/admin/comentarios' className='elemento-dropdown'>
            Comentarios Pendientes
          </Link>
        </>
      );
    } else if (rolDeUsuario === 'ADMIN_ORDENES') {
      return (
        <>
          <Link to='/admin/ordenes' className='elemento-dropdown'>
            Panel de Órdenes
          </Link>
        </>
      );
    } else if (rolDeUsuario === 'ADMIN_RECURSOS_INF') {
      return (
        <>
          <Link to='/admin/recursos-informativos' className='elemento-dropdown'>
            Recursos Informativos
          </Link>
        </>
      );
    } else if (rolDeUsuario === 'ADMINISTRADOR') {
      return (
        <>
          <Link to='/admin/usuarios' className='elemento-dropdown'>
            Usuarios
          </Link>
        </>
      );
    }
  }

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

          { obtenerOpcionesSegunRol(rolDeUsuario) }

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
    <nav className='navbar'>
      <div className='container'>
        <div className='logo-container'>
          <Link className='logo contraste-primario' to='/' style={{ textDecoration: 'none' }}>Hydrate</Link>
        </div>

        <ul className='nav-links'>
          <li>
            <Link className='nav-link' to='/about' style={{ textDecoration: 'none' }}>Sobre Nosotros</Link>
          </li>
          <li>
            <Link className='nav-link' to='/productos' style={{ textDecoration: 'none' }}>Productos</Link>
          </li>
          <li>
            <Link className='nav-link' to='/guias-usuario' style={{ textDecoration: 'none' }}>Guías de Usuario</Link>
          </li>
          <li>
            <Link className='nav-link' to='/datos-abiertos' style={{ textDecoration: 'none' }}>Datos Abiertos</Link>
          </li>
        </ul>
        
        <div className='btns-container'>
          { token === undefined || token === null ? botonesRegistro : btnDropdown }
        </div>
      </div>
    </nav>
  );
}
