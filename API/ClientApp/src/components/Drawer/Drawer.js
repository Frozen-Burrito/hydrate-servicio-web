import React from "react";

import './menu-drawer.css';

Drawer.defaultProps = {
  encabezado: null,
  elementos: [],
  elementoFinal: null,
  colorFondo: "primario",
  lado: "left",
  mostrar: false,
  conFondo: false,
};

export default function Drawer(props) {

  const { 
    encabezado, 
    mostrar, 
    elementos, 
    elementoFinal, 
    onCerrar,
    lado, 
    conNavbar,
    conFondo, 
    colorFondo
  } = props;

  const claseColor = colorFondo;
  const clasePadNavbar = conNavbar ? "pt-5" : "";
  const claseMostrar = mostrar ? "activo" : "escondido";
  const claseLado = convertirLado(lado);

  function convertirLado(lado) {
    switch(lado) {
      case "derecha":
        return "right";
      case "izquierda":
      default: 
        return "left"
    }
  }

  return (
    <div className={`contenedor-drawer ${claseMostrar} ${claseLado}`} onClick={() => false}>
      { conFondo && 
        <div 
          className="overlay-drawer"
          onClick={onCerrar} 
        /> 
      }
      
      <div className={`menu-drawer ${claseColor} ${clasePadNavbar}`}>
        <div className="drawer-superior">
          { encabezado != null && <h5>{ encabezado }</h5>}

          <>
            {elementos}
          </>
        </div>
        <div className="drawer-inferior">
          {elementoFinal}
        </div>
      </div>
    </div>
  );
}