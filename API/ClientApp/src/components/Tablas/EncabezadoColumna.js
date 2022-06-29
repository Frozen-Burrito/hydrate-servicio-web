import React, { useState } from "react";

/**
 * Crea un componente para un encabezado de columna de una tabla, 
 * usando un <th> como base. Puede ser presionado y puede tener 
 * un ícono.
 * @param {{ 
 * texto: string, 
 * onClick: function(React.MouseEvent): void, 
 * iconoActivo: string, 
 * iconoInactivo: string}} props Parámetros para el componente
 * @returns Un encabezado de columna para una tabla.
 */
export default function EncabezadoColumna(props) {

  const { texto, onClick, iconoActivo, iconoInactivo } = props;

  const [estaActiva, setEstaActiva] = useState(false);

  const manejarClickColumna = (e) => {
    onClick(e);

    if (iconoActivo != null && iconoInactivo != null) {
      setEstaActiva(!estaActiva);
    }
  }

  return (
    <th 
      onClick={(e) => manejarClickColumna() }
    >
      <div className="stack horizontal justify-between gap-1">
        <p>{ texto }</p>

        <span className={`material-icons`}>
          { estaActiva ? iconoActivo : iconoInactivo }
        </span>
      </div>
    </th>
  );
}