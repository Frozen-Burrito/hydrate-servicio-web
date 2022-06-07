import React from "react";

import { Layout, Avatar, DrawerPerfil } from "../../components";

export function PaginaTableroPerfil() {
  return (
    <Layout>
      <DrawerPerfil indiceItemActivo={1}/>

      <div className="panel-contenido">
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Tablero</h2>

          <div style={{ display: "flex" }}>
            <Avatar alt="Juan Perez" />
          </div>
        </div>
      </div>
    </Layout>
  );
}