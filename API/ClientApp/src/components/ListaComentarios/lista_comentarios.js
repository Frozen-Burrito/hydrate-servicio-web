import React, { useState, useEffect } from "react";

import useCookie from "../../utils/useCookie";

import { StatusHttp } from "../../api/api";
import * as api from "../../api/api_comentarios";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

import { TarjetaComentario, SearchBox, ControlPaginas } from "../";

ListaComentarios.defaultProps = {
  idAutor: null,
  pendientes: false,
  conBusqueda: true,
}

export default function ListaComentarios({ idAutor, pendientes, conBusqueda }) {

  const { valor: jwt } = useCookie("jwt");

  // La lista con todos los comentarios publicados obtenidos.
  const [comentarios, setComentarios] = useState([]);
  const [motivosDeRemovidos, setMotivosDeRemovidos] = useState([]);

  const [comentariosFiltrados, setComentariosFiltrados] = useState([]);

  // El estado de carga de los comentarios.
  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);
  const [buscando, setBuscando] = useState(false);

  const [paginaActual, setPaginaActual] = useState(1);
  const [paginasTotales, setPaginasTotales] = useState(1);

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

        const hayComentariosRemovidos = (motivosDeRemovidos.length > 0 && comentarios.find(c => !c.publicado) !== undefined);
        const idsComentariosRemovidos = [];
        
        if (hayComentariosRemovidos) {
          motivosDeRemovidos.forEach((registroMotivo) => {
            idsComentariosRemovidos.push(registroMotivo.idComentario);
          });
        }

        return comentariosFiltrados.map(comentario => {

          const fueRemovido = hayComentariosRemovidos ? idsComentariosRemovidos.includes(comentario.id) : false;

          const motivoRemovido = fueRemovido 
            ? motivosDeRemovidos.find(c => c.idComentario === comentario.id).motivo
            : null;

          return (
            <TarjetaComentario 
              key={comentario.id} 
              comentario={comentario}
              motivoDeReportes={motivoRemovido} 
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
      return ( <p>{`Aún no hay comentarios ${pendientes ? "pendientes" : "publicados"}.`}</p> );
    }
  }

  useEffect(() => {

    async function obtenerComentarios() {

      const obtenerDeAutor = (idAutor != null && idAutor.length > 0)
    
      // Obtener todos los comentarios pendientes. Ya vienen ordenados por
      // fecha desde la API.
      const resultado = pendientes
        ? await api.fetchComentariosPendientes(paginaActual, jwt)
        : (obtenerDeAutor 
          ? await api.fetchComentariosDeAutor(idAutor, paginaActual, jwt)
          : await api.fetchComentariosPublicados(paginaActual, jwt));
  
      if (resultado.status === StatusHttp.Status200OK) {

        const resultadoPaginado = resultado.datos;

        console.log(resultadoPaginado)
  
        setComentarios(resultadoPaginado.resultados);

        setPaginaActual(resultadoPaginado.paginaActual);
        setPaginasTotales(resultadoPaginado.paginasTotales);
  
        // Inicializar los comentarios filtrados con todos los comentarios
        // disponibles.
        setComentariosFiltrados(resultadoPaginado.resultados);

        setTieneError(false);
      } else {
        setTieneError(true)
      }
    }
  
    async function obtenerMotivosDeComentariosRemovidos() {
  
      const idUsuario = getIdUsuarioDesdeJwt(jwt);

      const resultado = await api.fetchMotivosComentariosRetirados(jwt, idUsuario, paginaActual);
  
      if (resultado.ok && resultado.status === StatusHttp.Status200OK) {
        
        setMotivosDeRemovidos(resultado.datos.resultados);
  
      } else {
        setTieneError(true)
      }
    }

    async function obtenerDatosComentarios() {
      
      setEstaCargando(true);

      const peticiones = [ obtenerComentarios() ];

      if (!pendientes && jwt != null) {
        peticiones.push(obtenerMotivosDeComentariosRemovidos());
      }
      
      await Promise.all(peticiones);
  
      setEstaCargando(false);
    }

    obtenerDatosComentarios();
  }, [ jwt, pendientes, idAutor, paginaActual ]);

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

      <div>
        <ControlPaginas
          paginasTotales={paginasTotales}
          paginaInicial={paginaActual}
          onAnterior={(e, nuevaPagina) => setPaginaActual(nuevaPagina)}
          onSiguiente={(e, nuevaPagina) => setPaginaActual(nuevaPagina)}
        />
      </div>
    </div>
  );
}
