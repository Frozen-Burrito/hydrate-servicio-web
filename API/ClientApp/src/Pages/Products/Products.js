import React from 'react'

import { Layout, ListaProductos } from '../../components';

export function Products () {

  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <h2 className="mt-3">Productos</h2>

        <ListaProductos />
      </section>
    </Layout>
  )
}