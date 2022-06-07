import React, { useState } from 'react';
import './dropdown.css';

export default function Dropdown({ boton, onColor, items }) {

  const [ mostrar, setMostrar ] = useState(false);

  const claseFondoAccion = mostrar ? "activo" : "inactivo";

  function toggleDropdown(e) {
    e.stopPropagation();
    e.preventDefault();

    setMostrar(!mostrar);
  }

  const getClaseContrasteBtn = (color) => {
    switch (color) {
      case "primario":
        return "contraste-primario";     
      case "superficie":
        return "contraste-superficie";
      default: return "contraste-fondo";
    }
  }

  return (
    <>
      <div className="dropdown">
        <button className={`dropdown-btn ${getClaseContrasteBtn(onColor)}`} onClick={toggleDropdown}>
          { boton }
        </button>

        <div className='dropdown-items' style={{display: mostrar ? 'flex' : 'none'}}>
          { items }
        </div>
      </div>

      <div className={`fondo-accion-dropdown ${claseFondoAccion}`} onClick={toggleDropdown}/>
    </>
  );
}