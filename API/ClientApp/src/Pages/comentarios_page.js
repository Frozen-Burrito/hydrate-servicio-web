import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";

import useCookie from "../utils/useCookie";
import Layout from "../components/Layout/Layout";

import { StatusHttp } from "../api/api";
import { fetchComentariosPublicados } from "../api/api_comentarios";

import Footer from "../components/Footer/Footer";
import SearchBox from "../components/SearchBox/searchbox";
import TarjetaComentario from "../components/TarjetaComentario/tarjeta_comentario";
import Tarjeta from "../components/Tarjeta/tarjeta";

/**
 * La página de vista general de comentarios, que muestra todos los comentarios
 * publicados, incluyendo sus respuestas.
 */
export function ComentariosPage() {

  const { valor: jwt } = useCookie("jwt");

  // La lista con todos los comentarios publicados obtenidos.
  const [comentarios, setComentarios] = useState([]);

  const [comentariosFiltrados, setComentariosFiltrados] = useState([]);

  // El estado de carga de los comentarios.
  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);
  const [buscando, setBuscando] = useState(false);

  const renderListaComentarios = () => {

    // Retornar placeholder de carga, si aún se estan cargando los 
    // comentarios.
    if (estaCargando) {
      return ( <p>Cargando comentarios...</p> );
    }
    
    // Mostrar placeholder mientras busca comentarios.
    if (buscando) {
      return ( <p>Encontrando el comentario perdido...</p>);
    }

    // Mostrar error, si lo hay.
    if (tieneError) {
      return ( <p>Error cargando los comentarios</p>);
    }

    // Si hay comentarios publicados, mostrar la lista de comentarios. 
    // Si aún no hay, mostrar un placeholder.
    if (comentarios.length > 0) {
      if (comentariosFiltrados.length > 0) {
        return comentariosFiltrados.map(comentario => {
          return (
            <TarjetaComentario 
              key={comentario.id} 
              comentario={comentario} 
              onComentarioEliminado={(idComentario) => {

                const comentariosRestantes = comentarios.filter(comentario => comentario.id !== idComentario); 

                setComentarios(comentariosRestantes);
                setComentariosFiltrados(comentariosRestantes)
              }}
            />
          );
        });
      } else {
        return ( <p>No se encontró ningún comentario para tu búsqueda.</p> );
      }
    } else {
      return ( <p>Aún no hay comentarios publicados.</p> );
    }
  }

  /**
   * Filtra los comentarios según un query, buscando coincidencias 
   * con el query en el asunto o contenido del comentario.
   * 
   * @param {string} query Un string de palabras clave.
   * @returns Los comentarios que incluyan el query en su asunto o contenido.
   */
  const filtrarComentarios = async (query) => {

    if (query.length <= 0) {
      setComentariosFiltrados(comentarios);
      return;
    }

    setBuscando(true);

    const resultados = comentarios.filter((comentario) => {
      const asuntoIncluyeQuery = comentario.asunto.toLowerCase().includes(query);
      const contenidoIncluyeQuery = comentario.contenido.toLowerCase().includes(query);

      return (asuntoIncluyeQuery || contenidoIncluyeQuery);
    });

    setComentariosFiltrados(resultados);

    setBuscando(false);

    return resultados;
  }

  useEffect(() => {

    async function obtenerComentarios() {
      setEstaCargando(true);

      // Obtener todos los comentarios publicados. Ya vienen ordenados por
      // fecha desde la API.
      const resultado = await fetchComentariosPublicados(jwt);

      if (resultado.ok && resultado.status === StatusHttp.Status200OK) {

        setComentarios(resultado.cuerpo);

        // Inicializar los comentarios filtrados con todos los comentarios
        // disponibles.
        setComentariosFiltrados(resultado.cuerpo);
      } else {
        setTieneError(true)
      }

      setEstaCargando(false);
    }

    obtenerComentarios();

  }, [ jwt ]);

  return (
    <Layout>
      <section className='contenedor full-page py-5'>
        <h2 className="mt-3">Comentarios de Usuarios</h2>

        <div className="columnas-flex">
          <div>
            <div className="stack horizontal justify-start gap-2 my-3">
              <SearchBox 
                icono="search" 
                iconoSufijo="clear"
                label="Buscar comentarios o respuestas" 
                onBusqueda={filtrarComentarios}
              />

              { /* Dropdown para filtrar */}
            </div>

            <div className="lista-comentarios">
              { renderListaComentarios() }
            </div>
          </div> { /* Columna principal */}

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
