import React from "react";
import { Link } from "react-router-dom";

FilaTabla.defaultProps = {
  registro: [],
  onEditar: null,
  onEliminar: null
};

export function FilaTabla ({ registro, onEditar, onEliminar }) {

  function renderCampos() {
    return (
      registro.map(({ valor, tipo }) => {
        
        if (tipo === "url") {
          return <Link to={valor}>{valor}</Link>
        }

        if (tipo === "dropdown") {
          
        }

        return (<td>{valor}</td>);
      })
    );
  }

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
    <tr key={registro.id}>
      { renderCampos() }
      <td className="acciones-fila">
        { renderAcciones }
      </td>
    </tr>
  );
}