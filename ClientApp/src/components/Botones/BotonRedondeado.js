import './Botones.css';
import React from 'react'
import { Link } from 'react-router-dom';

const BotonRedondeado = ({ relleno, texto, link }) => {
  return (
    <Link 
      className={`btn fuente-sm redondo btn-contraste${relleno ? '-relleno' : '-transparente'}`} 
      to={link} 
      style={{ textDecoration: 'none' }}
    >
      {texto}
    </Link>
  );
}

export default BotonRedondeado;