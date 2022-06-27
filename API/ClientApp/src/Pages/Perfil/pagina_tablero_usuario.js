import React, { useState } from "react";

import { Layout, Avatar, DrawerPerfil } from "../../components";

export function PaginaTableroPerfil() {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={1}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />

      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h3>Tablero</h3>

          <div style={{ display: "flex" }}>
            <Avatar alt="Juan Perez" />
          </div>
        </div>
      </section>
    </Layout>
  );
}