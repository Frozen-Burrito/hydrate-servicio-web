import React from "react";
import { useParams } from "react-router-dom";
import FormPublicarComentario from "../components/formularios/form_publicar_comentario";

import Layout from "../components/Layout/Layout";

/**
 * La página donde el usuario puede publicar nuevos comentarios de 
 * retroalimentacion con su experiencia del producto.
 */
export function PublicarComentarioPage() {

    const { idComentario } = useParams();

    return (
        <Layout>
            <main className='form-page center'>
                <FormPublicarComentario 
                    idComentario={idComentario}
                    esRespuesta={false}
                />

                <div className='img-fondo img-enviar-comentario' />
            </main>
        </Layout>
    );
}
