import './BotonesCompra.css';
import { Link } from 'react-router-dom';
import React from 'react'

const BotoneCompra = () => {
  return (
    <Link className='btn btn-secundario' to='/' style={{ textDecoration: 'none' }}>Comprar</Link>
  );
}

export default BotoneCompra;