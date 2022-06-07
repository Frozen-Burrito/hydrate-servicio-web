import React from 'react';

import { Layout, ListaComentarios, DrawerAdmin } from '../../components';

export function PaginaAdminComentarios () {

  return (
    <Layout>
      <DrawerAdmin indiceItemActivo={2} />

      <div className="panel-contenido ancho-max-70">
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Comentarios Pendientes de Revisión</h2>

          <h4>Ordenar</h4>
        </div>

        <ListaComentarios pendientes conBusqueda={false}/>
      </div>
    </Layout>
  )
}