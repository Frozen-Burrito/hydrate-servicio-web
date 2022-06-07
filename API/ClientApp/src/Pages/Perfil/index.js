import React from "react";

import { Layout, Avatar, DrawerPerfil } from "../../components";

export function PaginaPerfil() {
  return (
    <Layout>
      <DrawerPerfil indiceItemActivo={0}/>

      <div className="panel-contenido">
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Perfil del Usuario</h2>
        </div>

        <div style={{ display: "flex" }}>
          <Avatar alt="Juan Perez" />
        </div>
      </div>
    </Layout>
  );
}