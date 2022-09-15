import React from 'react'
import { Layout } from '../../components'
import DrawerDatosAbiertos from '../../components/DrawerConfigurados/drawer_datos_abiertos';

export default function PaginaCalidadDatosAbiertos() {
  return (
    <Layout>
        <DrawerDatosAbiertos
            lado="izquierda"
            mostrar={drawerVisible} 
            indiceItemActivo={1}
            onToggle={() => setDrawerVisible(!drawerVisible)}
        />

        <section className='contenedor full-page py-5'>
            <div className='stack horizontal justify-between gap-2 my-3'>

            </div>
        </section>
    </Layout>
  );
}
