import React, { useState, useEffect } from "react";

import { StatusHttp } from "../../api/api";
import { 
  fetchLlavesDeApiComoAdmin, 
  eliminarLlaveDeApi,
  fetchLlavesDeApiDelUsuario,
  regenerarLlave, 
} from "../../api/api_llaves";
import useCookie from "../../utils/useCookie";

import { 
  Tabla, 
  FilaTabla,
  EncabezadoColumna, 
  ControlPaginas,
  BotonIcono,
} from "..";

TablaLlavesDeApi.defaultProps = {
  esDeAdmin: false,
};

const llaveOfuscada = "************************************";

export default function TablaLlavesDeApi(props) {

  const { esDeAdmin } = props;

  const { valor: jwt } = useCookie("jwt");

  // Colección de órdenes de la página, para ser mostradas en la tabla. 
  const [llaves, setLlaves] = useState([]);

  const [mapaDeVisibilidad, setMapaDeVisibilidad] = useState(null);

  // Estados de carga y error de la tabla.
  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);

  // Control de resultados paginados.
  const [paginaActual, setPaginaActual] = useState(1);
  const [paginasTotales, setPaginasTotales] = useState(1);

  function manejarCambioPagina(e, nuevaPagina) {
    if (esDeAdmin) {
      if ((nuevaPagina >= 1 && nuevaPagina <= paginasTotales) || paginasTotales == null) {
        setPaginaActual(nuevaPagina);
      }
    }
  }

  // Columnas.
  const datosColumnas = esDeAdmin 
  ? [
    { texto: "ID", onclick: null}, 
    { texto: "Nombre de la llave", onclick: () => {}}, 
    { texto: "Usuario propietario", onclick: () => {}}, 
    { texto: "Número de Peticiones", onclick: () => {}}, 
    { texto: "Estado de la llave", onclick: () => {}}, 
    { texto: "Acciones", onclick: null}, 
  ] 
  : [
    { texto: "Cliente", onclick: () => {}}, 
    { texto: "Token", onclick: () => {}}, 
    { texto: "Número de Peticiones", onclick: () => {}}, 
    { texto: "Acciones", onclick: null}, 
  ];

  async function handleEliminarLlave(idLlaveParaEliminar, idPropietario = null) {

    const resultado = await eliminarLlaveDeApi(idLlaveParaEliminar, jwt, idPropietario);

    if (resultado.ok && resultado.status === StatusHttp.Status204SinContenido) {

      const llavesRestantes = llaves.filter(llave => llave.id !== idLlaveParaEliminar);

      setLlaves(llavesRestantes);
    }
  }

  function onCopiarLlave(valorLlave) {
    navigator.clipboard.writeText(valorLlave);
  }

  function onRevelarLlave(idLlave) {
    const nuevoValor = !mapaDeVisibilidad.get(idLlave);

    const mapaModificado = new Map(mapaDeVisibilidad); 
    
    mapaModificado.set(idLlave, nuevoValor);

    setMapaDeVisibilidad(mapaModificado);
  }

  async function onRegenerarLlave(idLlave) {

    if (!esDeAdmin) {
      const resultado = await regenerarLlave(idLlave, jwt);
  
      if (resultado.ok && resultado.status === StatusHttp.Status200OK) {
  
        const llaveRegenerada = resultado.cuerpo;
  
        setLlaves(prev => prev.map(llave => {
          if (llave.id === idLlave) {
            llave.llave = llaveRegenerada.llave
          }
  
          return llave;
        }));
      }
    }
  }

  useEffect(() => {

    async function obtenerLlavesComoAdmin() {

      setEstaCargando(true);

      const resultado = await fetchLlavesDeApiComoAdmin(
        paginaActual,
        jwt
      );

      if (resultado.ok && resultado.status === 200) {

        const resultadoPaginado = resultado.datos;

        setLlaves(resultadoPaginado.resultados);

        setPaginaActual(resultadoPaginado.paginaActual);
        setPaginasTotales(resultadoPaginado.paginasTotales);

      } else if (resultado.status >= 500) {
        console.log(resultado);

      } else if (resultado.status >= 400) {

        console.log(resultado);
      }

      setEstaCargando(false);
    }

    async function obtenerLlavesComoUsuario() {
      
      setEstaCargando(true);

      const resultado = await fetchLlavesDeApiDelUsuario(jwt);

      if (resultado.status === StatusHttp.Status200OK) {

        const datos = resultado.cuerpo;

        setLlaves(datos);
        
        const mapaVisibilidad = new Map(datos.map(llave => [llave.id, false]));
        setMapaDeVisibilidad(mapaVisibilidad);

        setTieneError(false);
        
      } else {
        setTieneError(true);
      }

      setEstaCargando(false);
    }

    if (esDeAdmin) {
      obtenerLlavesComoAdmin();
    } else {
      obtenerLlavesComoUsuario();
    }
    
  }, [jwt, paginaActual, esDeAdmin]);

  function renderTablaLlaves() {

    // Retornar placeholder de carga, si aún se estan cargando las 
    // llaves.
    if (estaCargando) {
      return ( <p>Cargando llaves de API...</p> );
    }
    
    // Mostrar error, si lo hay.
    if (tieneError) {
      return ( <p>Error cargando las llaves de API</p>);
    }

    // Si hay llaves de API registradas, mostrar la lista. 
    // Si aún no hay, mostrar un placeholder.
    if (llaves.length > 0) {
      return (
        <Tabla 
          resultadosPorPagina={25}
          columnas={datosColumnas.map((col, indice) => (
              <EncabezadoColumna
                key={ indice }
                texto={ col.texto }
                onClick={ col.onclick }
                iconoActivo={ col.onclick != null ? "" : null }
                iconoInactivo={ col.onclick != null ? "" : null }
              />
            )) 
          }>
          
          { llaves.map(llave => {

            const valorEsVisible = !esDeAdmin ? mapaDeVisibilidad.get(llave.id) : false;

            return (
              <FilaTabla key={llave.id}>
                  { esDeAdmin 
                    ? (
                      <>
                        <td>{ llave.id }</td>
                        <td>{ llave.nombre }</td>
                        <td>
                          <a href={`mailto:${llave.emailPropietario}`}>
                            { llave.nombrePropietario }
                          </a>
                        </td>
                        <td>{ llave.numeroDePeticiones.toString() }</td> 
                        <td>{ llave.tuvoActividadEnMesPasado ? "Activa" : "Inactiva" }</td> 
                        <td className="stack horizontal justify-start gap-1">
                          <BotonIcono 
                            icono="clear"
                            tipo="fill"
                            color="primario"
                            onClick={() => handleEliminarLlave(llave.id, llave.idPropietario)}
                          />                     
                        </td>
                      </>
                    ) : (
                      <>
                        <td>{ llave.nombre }</td>
                        <td>{ valorEsVisible ? llave.llave : llaveOfuscada }</td>
                        <td>{ llave.peticionesEnMes.toString() }</td> 
                        <td className="stack horizontal justify-start gap-1">
                          <BotonIcono 
                            icono="content_copy"
                            tipo="texto"
                            onClick={() => onCopiarLlave(llave.llave)}
                          />
                          <BotonIcono 
                            icono={ valorEsVisible ? "visibility_off" : "visibility" }
                            tipo="texto"
                            onClick={() => onRevelarLlave(llave.id)}
                          />
                          <BotonIcono 
                            icono="sync"
                            tipo="texto"
                            onClick={() => onRegenerarLlave(llave.id)}
                          />

                          <BotonIcono 
                            icono="clear"
                            tipo="fill"
                            color="primario"
                            onClick={() => handleEliminarLlave(llave.id)}
                          />
                        </td>          
                      </>
                    )}

              </FilaTabla>
            );
          })}

        </Tabla>
      );
    } else {
      return ( <p>Todavía no se han registrado llaves de API.</p> );
    }
  }

  return (
    <>
      { renderTablaLlaves() }

      { esDeAdmin && 
        <div className="mt-5">
          <ControlPaginas
            paginasTotales={paginasTotales}
            paginaInicial={paginaActual}
            onAnterior={manejarCambioPagina}
            onSiguiente={manejarCambioPagina}
          />
        </div>
      }

    </>
  );
}