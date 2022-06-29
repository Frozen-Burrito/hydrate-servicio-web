import React from "react";

import './tablas.css';

Tabla.defaultProps = {
  columnas: ( <th>No hay columnas aun.</th> ),
  children: null
};

export default function Tabla(props) {

  const { 
    columnas,
    children,
  } = props;

  return (
    <>
      <table className="mt-5">
        <thead>
          <tr className="separador-inf">
            { columnas }
          </tr>
        </thead>
        <tbody>
          { children }
        </tbody>
      </table>
    </>
  );
}