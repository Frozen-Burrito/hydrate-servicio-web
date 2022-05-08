import React from 'react';
import './Tarjeta.css';

const Tarjeta = ({ children }) => {
  return (
    <div className='card'>
      {children}
    </div>
  );
}

export default Tarjeta;
