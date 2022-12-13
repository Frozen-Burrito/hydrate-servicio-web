import React from "react";

import { Link } from "react-router-dom";
import { Layout, ListaComentarios, Footer, Tarjeta } from "../components";

/**
 * La página de vista general de comentarios, que muestra todos los comentarios
 * publicados, incluyendo sus respuestas.
 */
export function ComentariosPage() {

  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <div className="my-3 stack horizontal justify-between">
          <h2>Comentarios de Usuarios</h2>

          <Link 
            className={`btn btn-primario`}
            style={{ textDecoration: 'none' }}
            to="/comentarios/publicar"
          >
            Publicar Comentario
          </Link>
        </div>

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
