import React, { useState } from 'react';
import { Layout, DrawerDatosAbiertos } from '../../components';

export function PaginaMetasDatosAbiertos() {

    const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
        <DrawerDatosAbiertos
            lado="izquierda"
            mostrar={drawerVisible}
            indiceItemActivo={2}
            onToggle={() => setDrawerVisible(!drawerVisible)}
        />

        <section className='contenedor full-page py-5'>
            <div className='stack horizontal justify-between gap-2 my-3'>

            </div>
        </section>
    </Layout>
  );
}
