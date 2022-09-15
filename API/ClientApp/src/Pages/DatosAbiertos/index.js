import React, { useState } from 'react'
import { Layout, DrawerDatosAbiertos } from '../../components';

export function DatosAbiertos () {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
        <DrawerDatosAbiertos
          lado="izquierda"
          mostrar={drawerVisible}
          indiceItemActivo={0}
          onToggle={() => setDrawerVisible(!drawerVisible)}
        />

        <section className='contenedor full-page py-5' >
          <div className='stack horizontal gap-2 my-2' >

          </div>
        </section>
    </Layout>
  );
}
