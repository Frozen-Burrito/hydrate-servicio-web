import React from "react";
import { useParams } from "react-router-dom";

import { Layout, DrawerPerfil, ListaComentarios } from "../../components";

export function PaginaComentariosPerfil() {

  const { idUsuario } = useParams();

  return (
    <Layout>
      <DrawerPerfil />
      
      <div className="panel-contenido">
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Comentarios Que Has Escrito</h2>

          <h4>Ordenar</h4>
        </div>

        <ListaComentarios idAutor={idUsuario} conBusqueda={false}/>
      </div>
    </Layout>
  );
}