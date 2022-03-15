import './Navbar.css';
import React from 'react'
import { Link } from 'react-router-dom';
import Botones from '../Botones/Botones';

// class ScrollAwareDiv extends React.Component {
//     constructor(props) {
//         super(props);
//         this.myRef = React.createRef();
//         this.state = {scrollTop: 0};
//     }

//     onScroll = () => {
//         const scrollY = window.scrollY;
//         const scrollTop = this.myRef.current.scrollTop;
//         console.log(`onScroll, window.scrollY: ${scrollY} myRef.scrollTop: ${scrollTop}`);
//         this.setState({
//             scrollTop,
//         });
//     }
// }

export default function Navbar() {
  return (
    <div className='box'>
        <nav className={`navbar`}>
            <div className='container'>
                <div className='logo-container'>
                    <Link exact className='logo' activeClassName='active' to='/' style={{ textDecoration: 'none' }}>Hydrate</Link>
                </div>
                <div className='links-container'>
                    <div className='nav-links'>
                        <ul>
                            <li className='link'>
                                <Link className='nav-link' to='/productos' style={{ textDecoration: 'none' }}>Productos</Link>
                            </li>
                            <li className='link'>
                                <Link className='nav-link' to='/guias-usuario' style={{ textDecoration: 'none' }}>Guías de Usuario</Link>
                            </li>
                            <li className='link'>
                                <Link className='nav-link' to='/datos-abiertos' style={{ textDecoration: 'none' }}>Datos Abiertos</Link>
                            </li>
                            <li className='link'>
                                <Link className='nav-link' to='/about' style={{ textDecoration: 'none' }}>Sobre Nosotros</Link>
                            </li>
                        </ul>
                    </div>
                    <div className='btns-container'>
                        <Botones />
                        {/* <Link className='btn transparent' to='/' style={{ textDecoration: 'none' }}>Log in</Link>
                        <Link className='btn solid' to='/' style={{ textDecoration: 'none' }}>Sign up</Link> */}
                    </div>
                </div>
            </div>
        </nav>
    </div>
  );
}
