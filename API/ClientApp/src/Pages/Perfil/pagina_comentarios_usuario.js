import React, { useState } from "react";
import { useParams } from "react-router-dom";

import { Layout, DrawerPerfil, ListaComentarios } from "../../components";

export function PaginaComentariosPerfil() {

  const { idUsuario } = useParams();

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={2}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />
      
      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Comentarios Que Has Escrito</h2>

          <h4>Ordenar</h4>
        </div>

        <ListaComentarios idAutor={idUsuario} conBusqueda={false}/>
      </section>
    </Layout>
  );
}