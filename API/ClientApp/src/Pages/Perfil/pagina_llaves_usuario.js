import React, { useState } from "react";
import { Link } from "react-router-dom";

import { Layout, DrawerPerfil, TablaLlaves } from "../../components";

export function PaginaLlavesUsuario() {

  const [drawerVisible, setDrawerVisible] = useState(false);

  return (
    <Layout>
      <DrawerPerfil 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={4}
        onToggle={() => setDrawerVisible(!drawerVisible)}
      />
      
      <section className='contenedor full-page py-5'>
        <div className="stack horizontal justify-between gap-2 my-3">
          <h2>Tokens Para Clientes de API</h2>

					<Link 
						to={ "/llaves/nueva" } 
						className={`btn btn-primario`}
					>
						Nuevo Token
					</Link>
        </div>

        <TablaLlaves />
      </section>
    </Layout>
  );
}