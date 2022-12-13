import React from 'react'
import { Link } from 'react-router-dom';

const BotonesGenerales = ( {texto, url} ) => {
  return (
    <Link className='btn btn-primario' to={url} style={{ textDecoration: 'none', fontSize: '20px', textAlign: 'center' }}>{ texto }</Link>
  );
}

export default BotonesGenerales;