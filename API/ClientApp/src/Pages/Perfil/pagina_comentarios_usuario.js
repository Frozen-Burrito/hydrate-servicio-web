import React from "react";

import Layout from "../../components/Layout/Layout";
import Avatar from "../../components/Avatar/avatar";

export function PaginaComentariosPerfil() {
  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <h2 className="mt-3">Comentarios</h2>

        <div style={{ display: "flex" }}>
          <Avatar alt="Juan Perez" />
        </div>
      </section>
    </Layout>
  );
}