import React, { useState } from "react";

import { Layout, DrawerPerfil } from "../../components";
import { Cards } from "../../components/Cards/Cards";

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
        <div className="stack horizontal justify-between gap-2 my-3" style={{ display: "flex", flexDirection: "column" }}>
          <h3>Datos de consumo del usuario</h3>
          <Cards />
        </div>
      </section>
    </Layout>
  );
}