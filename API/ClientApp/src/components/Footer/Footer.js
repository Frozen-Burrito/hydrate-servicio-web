import React from 'react'
import './Footer.css';
import { Link } from 'react-router-dom';
import BotonesGenerales from '../../components/Botones/BotonesGenerales';

const texto = 'Iniciar Sesión';

const Footer = () => {
  return (
    <footer className='footer-section'>
        <div className='footer-box'>
          <div className='footer-navbar'>
            <div className='footer-title-container'>
              <h3 className='footer-title'>Hydrate</h3>
            </div>
            <div className='footer-links-container'>
              <div className='product-links'>
                <h4 className='links-footer-title'>Botella</h4>
                <Link className='footer-link' to='nowhere' style={{ textDecoration: 'none' }}>Características</Link>
                <Link className='footer-link' to='nowhere' style={{ textDecoration: 'none' }}>Comprar</Link>
                <Link className='footer-link' to='nowhere' style={{ textDecoration: 'none' }}>Descargar la App</Link>
              </div>
              <div className='comunity-links'>
                <h4 className='links-footer-title'>Comunidad</h4>
                <Link className='footer-link' to='/guias-usuario' style={{ textDecoration: 'none' }}>Guías de Usuario</Link>
                <Link className='footer-link' to='nowhere' style={{ textDecoration: 'none' }}>Comentarios</Link>
                <Link className='footer-link' to='/datos-abiertos' style={{ textDecoration: 'none' }}>Datos Abiertos</Link>
              </div>
              <div className='company-links'>
                <h4 className='links-footer-title'>Compañia</h4>
                <Link className='footer-link' to='/about' style={{ textDecoration: 'none' }}>Sobre Nosotros</Link>
                <Link className='footer-link' to='nowhere' style={{ textDecoration: 'none' }}>Contáctanos</Link>
              </div>
            </div>
            <div className='footer-btn-container'>
              <BotonesGenerales texto={texto} />
            </div>
          </div>
          <hr />
          <div className='footer-company'>
            <div className='footer-company-name-container'>
              <p className='footer-company-name'>© 2021 Nombre del Proyecto</p>
            </div>
            <div className='footer-social-media'>
              <div className='social-media-text-container'>
                <p className='social-media-text'>Siguenos en redes sociales:</p>
              </div>
              <div className='social-media-icons'>

              </div>
            </div>
          </div>
        </div>
    </footer>
  );
}

export default Footer;
