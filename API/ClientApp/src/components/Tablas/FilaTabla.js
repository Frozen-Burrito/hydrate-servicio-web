import React from "react";

FilaTabla.defaultProps = {
  children: <td>No hay registros en la fila.</td>,
  onEditar: null,
  onEliminar: null
};

export default function FilaTabla ({ children, onEditar, onEliminar }) {

  function renderAcciones() {
    if (onEditar != null || onEliminar != null) {
      
      return (
        <td className="acciones-fila">
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
        </td>
      );
    }
  }

  return (
    <tr>
      { children }

      { renderAcciones() }
    </tr>
  );
}