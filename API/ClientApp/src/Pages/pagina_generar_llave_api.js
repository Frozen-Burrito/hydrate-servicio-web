import React from "react";

import { Layout, FormObtenerLlave } from "../components";

/**
 * La página donde el usuario puede obtener una nueva llave de API.
 */
export function PaginaGenerarLlaveAPI() {

    return (
        <Layout>
            <main className='form-page center'>
                <FormObtenerLlave />

                <div className='img-fondo img-enviar-comentario' />
            </main>
        </Layout>
    );
}
