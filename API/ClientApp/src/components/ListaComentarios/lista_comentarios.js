import React, { useState, useEffect } from "react";

import useCookie from "../../utils/useCookie";

import { StatusHttp } from "../../api/api";
import { 
  fetchComentariosPublicados,
  fetchComentariosDeAutor 
} from "../../api/api_comentarios";

import { TarjetaComentario, SearchBox } from "../";

export default function ListaComentarios({ idAutor, conBusqueda }) {

  const { valor: jwt } = useCookie("jwt");

  // La lista con todos los comentarios publicados obtenidos.
  const [comentarios, setComentarios] = useState([]);

  const [comentariosFiltrados, setComentariosFiltrados] = useState([]);

  // El estado de carga de los comentarios.
  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);
  const [buscando, setBuscando] = useState(false);

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

  useEffect(() => {

    async function obtenerComentarios() {
      setEstaCargando(true);

      console.log(idAutor);

      // Obtener todos los comentarios publicados. Ya vienen ordenados por
      // fecha desde la API.
      const resultado = idAutor != null && idAutor.length > 0 
        ? await fetchComentariosDeAutor(idAutor, jwt)
        : await fetchComentariosPublicados(jwt);

      if (resultado.ok && resultado.status === StatusHttp.Status200OK) {

        setComentarios(resultado.cuerpo);

        // Inicializar los comentarios filtrados con todos los comentarios
        // disponibles.
        setComentariosFiltrados(resultado.cuerpo);
      } else {
        setTieneError(true)
      }

      console.log(resultado);

      setEstaCargando(false);
    }

    obtenerComentarios();

  }, [ jwt ]);

  return (
    <div>
      <div className="stack horizontal justify-start gap-2 my-3">
        { conBusqueda && (
          <SearchBox 
            icono="search" 
            iconoSufijo="clear"
            label="Buscar comentarios o respuestas" 
            onBusqueda={filtrarComentarios}
          />
        )}

        { /* Dropdown para filtrar */}
      </div>
      <div className="lista-comentarios">
        { renderListaComentarios() }
      </div>
    </div>
  );
}
