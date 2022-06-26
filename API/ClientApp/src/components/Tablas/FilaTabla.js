import React from "react";
import { Link } from "react-router-dom";

FilaTabla.defaultProps = {
  children: <td>No hay registros en la fila.</td>,
  onEditar: null,
  onEliminar: null
};

export default function FilaTabla ({ children, onEditar, onEliminar }) {

  function renderAcciones() {
    return (
      <>
        {onEditar != null && (
          <button className="btn-editar-fila" onClick={onEditar}>
            <span className="material-icons">
              edit
            </span>
          </button>
        )}

        { onEliminar != null && (
          <button className="btn-eliminar-fila" onClick={onEliminar}>
            <span className="material-icons">
              clear
            </span>
          </button>
        )}
      </>
    );
  }

  return (
    <tr>
      { children }

      <td className="acciones-fila">
        { renderAcciones() }
      </td>
    </tr>
  );
}