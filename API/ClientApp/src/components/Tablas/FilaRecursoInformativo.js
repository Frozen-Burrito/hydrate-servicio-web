import React from "react";
import { Link } from "react-router-dom";

export const FilaRecursoInformativo = ({ recurso, onEditar, onEliminar }) => {

  return (
    <tr key={recurso.id}>
      <td>{recurso.titulo}</td>
      <td>{recurso.fechaPublicacion.substring(0, 10)}</td>
      <td>
        <Link to={recurso.url}>{recurso.url}</Link>
      </td>
      <td>{recurso.descripcion}</td>
      <td className="acciones-fila">
        <button className="btn-editar-fila" onClick={onEditar}>
          <span className="material-icons">
            edit
          </span>
        </button>
        <button className="btn-eliminar-fila" onClick={onEliminar}>
          <span className="material-icons">
            clear
          </span>
        </button>
      </td>
    </tr>
  );
}