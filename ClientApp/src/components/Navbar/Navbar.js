import './Navbar.css';
import React from 'react'
import { Link } from 'react-router-dom';
import Botones from '../Botones/Botones';

const transparent = 'transparent';
const solid = 'solid';
const signUp = "Sign up";
const logIn = "Log in";
const inicioSesion = '/inicio-sesion';
const creacionCuenta = '/creacion-cuenta';

export default function Navbar() {
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
                    <Botones property={transparent} texto={logIn} link={inicioSesion} />
                    <Botones property={solid} texto={signUp} link={creacionCuenta} />
                </div>
            </div>
        </div>
    </nav>
  );
}
