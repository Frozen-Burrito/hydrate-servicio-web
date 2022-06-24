import React from "react";
import { FilaRecursoInformativo } from "./FilaRecursoInformativo";
import { FilaTabla } from "./FilaTabla";

import './tablas.css';

export default function Tabla({ 
  columnas,
  evtsOnClickColumnas,
  registros, 
  onEditarRegistro, 
  onEliminarRegistro,
  estaCargando,
  tieneError 
}) {

  if (registros.length > 0 && columnas.length !== registros[0].length) {
    return (
      <p></p>
    )
  }

  return (
    <table className="mt-5">
      <thead>
        <tr className="separador-inf">
          <th>Título</th>
          <th>Fecha de Publicación</th>
          <th>URL</th>
          <th>Descripción</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        { registros.map((registro, indice) => {
          return (
            <FilaTabla 
              key={recurso.id}
              recurso={recurso}
              onEditar={() => onEditarRecurso(recurso.id)}
              onEliminar={() => onEliminarRecurso(recurso.id)}
            />
          );
        })}
      </tbody>
    </table>
  );
}