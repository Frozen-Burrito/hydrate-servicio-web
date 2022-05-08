import './menu-drawer.css';
import React from "react";

export const Drawer = ({ encabezado, elementos, elementoFinal }) => {

  return (
    <div className="menu-drawer">
      <div className="drawer-superior">
        <h5>{encabezado}</h5>

        <>
          {elementos}
        </>
      </div>
      <div className="drawer-inferior">
        {elementoFinal}
      </div>
    </div>
  );
}