import React from "react";
import { useParams } from "react-router-dom";
import FormPublicarComentario from "../components/formularios/form_publicar_comentario";

import Layout from "../components/Layout/Layout";

/**
 * La página donde el usuario puede publicar una respuesta a 
 * un comentario.
 */
export function ResponderComentarioPage() {

    const { idComentario } = useParams();

    return (
        <Layout>
            <main className='form-page center'>
                <FormPublicarComentario 
                    idComentario={idComentario}
                    esRespuesta={true}
                />

                <div className='img-fondo img-enviar-comentario' />
            </main>
        </Layout>
    );
}
