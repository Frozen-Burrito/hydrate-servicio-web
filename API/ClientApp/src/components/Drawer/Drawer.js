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
  capa: 6,
  conPadding: true,
  btnCerrarExterno: null,
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
    conPadding,
    colorFondo,
    capa,
    btnCerrarExterno,
  } = props;

  const claseColor = colorFondo;
  const clasePadNavbar = conNavbar ? "pt-5" : "";
  const clasePadHorizontal = conPadding ? "px" : "";
  const claseMostrar = mostrar ? "activo" : "escondido";
  const claseCapa = `capa-${capa}`;
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

      { btnCerrarExterno && 
        <div className="wrap-btn-cerrar-drawer">
          { btnCerrarExterno }
        </div> 
      }
      
      <div className={`menu-drawer ${claseColor} ${clasePadNavbar} ${claseCapa} ${clasePadHorizontal}`}>
        <div className="drawer-superior">
          { encabezado != null && 
            <h5 className={`encabezado-drawer ${conPadding ? "" : "ml-1"}`}>
              { encabezado }
            </h5>
          }

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