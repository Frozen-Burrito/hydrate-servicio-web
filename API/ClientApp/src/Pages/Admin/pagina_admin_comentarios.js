import React, { useState } from 'react';

import { Layout, ListaComentarios, DrawerAdmin } from '../../components';

export function PaginaAdminComentarios () {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerAdmin 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={2}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />

      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h3>Comentarios Pendientes de Revisión</h3>

          <h4>Ordenar</h4>
        </div>

        <ListaComentarios pendientes conBusqueda={false}/>
      </section>
    </Layout>
  )
}