import './Botones.css';
import React from 'react'
import { Link } from 'react-router-dom';

const Botones = ({ property, texto, link }) => {
  return (
    <Link className={`btn ${property}`} to={link} style={{ textDecoration: 'none' }}>{texto}</Link>
  );
}

export default Botones;