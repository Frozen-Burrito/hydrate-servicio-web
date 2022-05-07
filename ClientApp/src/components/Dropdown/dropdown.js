import React, { useState } from 'react';
import './dropdown.css';

export default function Dropdown({ boton, onColor, items }) {

  const [ mostrar, setMostrar ] = useState(false);

  function toggleDropdown() {
    setMostrar(!mostrar);
  }

  return (
    <div className='dropdown'>
      <button className='dropdown-btn' style={{ color: 'var(--contraste-primario-claro)'}} onClick={toggleDropdown}>
        { boton }
      </button>

      <div className='dropdown-items' style={{display: mostrar ? 'flex' : 'none'}}>
        { items }
      </div>
    </div>
  );
}