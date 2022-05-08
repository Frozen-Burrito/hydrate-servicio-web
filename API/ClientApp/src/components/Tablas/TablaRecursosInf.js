import React from "react";
import { FilaRecursoInformativo } from "./FilaRecursoInformativo";

import './tablas.css';

export const TablaRecursosInf = ({ 
  recursosInformativos, 
  onEditarRecurso, 
  onEliminarRecurso,
  estaCargando,
  tieneError 
}) => {

  return (
    <table className="mt-5">
      <tr className="separador-inf">
        <th>Título</th>
        <th>Fecha de Publicación</th>
        <th>URL</th>
        <th>Descripción</th>
        <th>Acciones</th>
      </tr>
      { recursosInformativos.map((recurso, _) => {
        return (
          <FilaRecursoInformativo 
            key={recurso.id}
            recurso={recurso}
            onEditar={() => onEditarRecurso(recurso.id)}
            onEliminar={() => onEliminarRecurso(recurso.id)}
          />
        );
      })}
    </table>
  );
}