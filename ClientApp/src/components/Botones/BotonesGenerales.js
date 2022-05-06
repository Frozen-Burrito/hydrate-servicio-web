import React from 'react'
import { Link } from 'react-router-dom';

const BotonesGenerales = ( {texto} ) => {
  return (
    <Link className='btn btn-primario' to='/' style={{ textDecoration: 'none' }}>{ texto }</Link>
  );
}

export default BotonesGenerales;