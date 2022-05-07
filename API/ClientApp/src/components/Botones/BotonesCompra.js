import './BotonesCompra.css';
import { Link } from 'react-router-dom';
import React from 'react'

const BotonesCompra = () => {
  return (
    <Link className='button-buys' to='/' style={{ textDecoration: 'none' }}>Comprar</Link>
  );
}

export default BotonesCompra;