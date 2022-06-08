import React from "react";

import { Layout, ListaComentarios, Footer, Tarjeta } from "../components";

/**
 * La página de vista general de comentarios, que muestra todos los comentarios
 * publicados, incluyendo sus respuestas.
 */
export function ComentariosPage() {

  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <h2 className="mt-3">Comentarios de Usuarios</h2>

        <div className="columnas-flex">
          <ListaComentarios conBusqueda />

          <div className="col-3">
            <Tarjeta
              titulo="Temas Recientes"
              elevacion={0}
            >
              <div> 
                {/* <Link to={} /> */}
                <p>Aún no hay temas recientes.</p>
              </div>
            </Tarjeta>
          </div> { /* Columna lateral de temas */}
        </div> { /* Contenido */}
      </section>

      <Footer />
    </Layout>
  );
}
