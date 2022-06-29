import React, { useState }from 'react'

import { DrawerCompra, Layout, ListaProductos } from '../../components';

export function Products () {

  const [mostrarDrawerCompra, setMostrarDrawerCompra] = useState(false);
  const [productoSeleccionado, setProductoSeleccionado] = useState(null);

  function handleClickCompra(producto) {
    setMostrarDrawerCompra(true);
    setProductoSeleccionado(producto);
  }

  function handleCancelarCompra() {
    setMostrarDrawerCompra(false);
    setProductoSeleccionado(null);
  }

  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <h2 className="mt-3">Productos</h2>

        <ListaProductos onComprarProducto={handleClickCompra}/>
      </section>

      <DrawerCompra
        mostrar={mostrarDrawerCompra}
        producto={productoSeleccionado}
        lado="derecha"
        onCancelarCompra={handleCancelarCompra}
      />
    </Layout>
  )
}