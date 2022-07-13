import React, { useState } from "react";

import { Layout, DrawerPerfil, TablaOrdenes } from "../../components";

export function PaginaOrdenesUsuario() {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={3}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />
      
      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Mis Órdenes de Compra</h2>
        </div>

        <TablaOrdenes />
      </section>
    </Layout>
  );
}