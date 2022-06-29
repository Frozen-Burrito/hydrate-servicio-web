import React, { useState } from "react";

import { Layout, Avatar, DrawerPerfil } from "../../components";

export function PaginaPerfil() {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={0}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />

      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h3>Perfil del Usuario</h3>
        </div>

        <div style={{ display: "flex" }}>
          <Avatar alt="Juan Perez" />
        </div>
      </section>
    </Layout>
  );
}