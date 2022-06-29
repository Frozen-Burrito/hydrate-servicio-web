import React, { useState } from 'react';

import { Layout, DrawerAdmin } from '../../components';

export function PaginaAdminUsuarios () {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerAdmin 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={0}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />
      
      <section className='contenedor full-page py-5'>
        <h3 className="mt-3">Usuarios</h3>

      </section>
    </Layout>
  )
}