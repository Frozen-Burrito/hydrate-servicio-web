import React, { useState, useEffect } from "react";

import { StatusHttp } from "../../api/api";
import { fetchOrdenes, fetchProductos } from "../../api/api_productos";
import useCookie from "../../utils/useCookie";

import { 
  SearchBox, 
  Tabla, 
  FilaTabla,
  EncabezadoColumna, 
  ControlPaginas,
  Dropdown,
  BotonIcono,
} from "../";

export default function TablaOrdenes() {

  const { valor: jwt } = useCookie('jwt');

  // Colección de órdenes de la página, para ser mostradas en la tabla. 
  const [ordenes, setOrdenes] = useState([]);

  // Un mapa con todos los productos, donde las llaves son el ID de cada producto.
  const [productos, setProductos] = useState(new Map());

  // Estados de carga y error de la tabla.
  const [estaCargando, setEstaCargando] = useState(false);
  const [tieneError, setTieneError] = useState(false);

  // Queries de búsqueda según el ID de la orden y los datos del cliente.
  const [queryDeOrdenes, setQueryDeOrdenes] = useState({
    idOrden: null,
    nombre: null,
    email: null,
  });

  // Filtros por fecha y estado de orden.
  const [filtroPorEstado, setFiltroPorEstado] = useState(null);
  const [filtroPorFechaInicial, setFiltroPorFechaInicial] = useState(null);
  const [filtroPorFechaFinal, setFiltroPorFechaFinal] = useState(null);

  // Control de resultados paginados.
  const [paginaActual, setPaginaActual] = useState(1);
  const [paginasTotales, setPaginasTotales] = useState(1);

  function manejarCambioPagina(e, nuevaPagina) {
    if ((nuevaPagina >= 1 && nuevaPagina <= paginasTotales) || paginasTotales == null) {
      setPaginaActual(nuevaPagina);
    }
  }

  // Columnas.
  const datosColumnas = [
    { texto: "ID de la Orden", onclick: null}, 
    { texto: "Fecha", onclick: () => {}}, 
    { texto: "Cliente", onclick: () => {}}, 
    { texto: "Monto", onclick: () => {}}, 
    { texto: "Productos", onclick: null}, 
    { texto: "Estado", onclick: null}, 
  ];

  const estadoDeOrden = [
    "Pendiente",
    "En Progreso",
    "Concluida",
    "Cancelada"
  ];
 
  /**
   * Actualiza el el query de órdenes.
   * @param {string} query El string de búsqueda del SearchBox.
   */
  function cambiarQueryOrdenes(query) {

    console.log(query);

    const queryStr = query.trim();

    if (queryStr.includes("@") && !queryStr.includes(" ")) {
      // El query es el email de un cliente.
      setQueryDeOrdenes({
        idOrden: null,
        nombre: null,
        email: queryStr,
      });

    } else if (queryStr.includes("-") && !queryStr.includes(" ")) {
      // El query es el ID de una orden.
      setQueryDeOrdenes({
        idOrden: queryStr,
        nombre: null,
        email: null,
      });

    } else {
      // El query es el nombre de un cliente.
      setQueryDeOrdenes({
        idOrden: null,
        nombre: queryStr,
        email: null,
      });
    }
  }

  function onCambioEstadoOrden(indiceNuevoEstado) {

  }

  useEffect(() => {

    async function obtenerOrdenes() {

      const params = {
        query: null,
        idCliente: null,
        nombreCliente: queryDeOrdenes.nombre, 
        email: queryDeOrdenes.email, 
        idOrden: queryDeOrdenes.idOrden,
        estadoOrden: filtroPorEstado,
      };

      const resultado = await fetchOrdenes(
        params,
        jwt, 
        paginaActual,
      );

      if (resultado.ok && resultado.status === 200) {

        const resultadoPaginado = resultado.datos;

        setOrdenes(resultadoPaginado.resultados);

        setPaginaActual(resultadoPaginado.paginaActual);
        setPaginasTotales(resultadoPaginado.paginasTotales);

      } else if (resultado.status >= 500) {
        console.log(resultado);

      } else if (resultado.status >= 400) {

        console.log(resultado);
      }
    }

    async function obtenerProductos() {

      const resultado = await fetchProductos(1);

      if (resultado.status === StatusHttp.Status200OK) {

        const datosPaginados = resultado.datos;

        const mapaProductos = new Map(datosPaginados.resultados.map(producto => [producto.id, producto]));

        setProductos(mapaProductos);
        setTieneError(false);
        
      } else {
        setTieneError(true);
      }
    }

    async function obtenerDatos() {
      
      setEstaCargando(true);

      console.log("Obteniendo ordenes...");

      const peticiones = [ obtenerOrdenes(), obtenerProductos() ];
      
      await Promise.all(peticiones);
  
      setEstaCargando(false);
    }

    obtenerDatos();
    
  }, [jwt, paginaActual, queryDeOrdenes, filtroPorEstado]);
  
  const renderDropdownEstado = (orden) => (
    <Dropdown 
      onColor="superficie"
      boton={(
        <BotonIcono 
          icono="pending_actions"
          label={ estadoDeOrden[orden.estado] }
          tipo="texto"
        />
      )}
      items={(
        <>
          { estadoDeOrden.map((estado, indice) => (
            <button key={indice} className ="elemento-dropdown" onClick={() => onCambioEstadoOrden(indice)}>
              { estado }
            </button>
          ))}
        </>
      )}
    />
  );

  function renderTablaOrdenes() {

    // Retornar placeholder de carga, si aún se estan cargando las 
    // ordenes.
    if (estaCargando) {
      return ( <p>Cargando órdenes...</p> );
    }
    
    // Mostrar error, si lo hay.
    if (tieneError) {
      return ( <p>Error cargando las órdenes</p>);
    }

    // Si hay órdenes de compra, mostrar la lista. 
    // Si aún no hay, mostrar un placeholder.
    if (ordenes.length > 0) {
      return (
        <Tabla 
          resultadosPorPagina={25}
          columnas={(
            <>
              { datosColumnas.map((col, indice) => (
                <EncabezadoColumna
                  key={ indice }
                  texto={ col.texto }
                  onClick={ col.onclick }
                  iconoActivo={ col.onclick != null ? "" : null }
                  iconoInactivo={ col.onclick != null ? "" : null }
                />
              )) } 

              <th>Acciones</th>
            </>
          )}>
          
          { ordenes.map(orden => {
            return (
              <FilaTabla 
                key={orden.id}
                // onEditar={() => onEditarRecurso(orden.id)}
                onEditar={() => console.log("TODO: editar orden")}>
                <td>{ orden.id }</td>
                <td>{ orden.fecha.substring(0, 10) }</td>
                <td>{ orden.idCliente }</td> 
                <td>${ orden.montoTotal.toFixed(2) } MXN</td> 
                
                <td className="stack vertical justify-start gap-1">
                  { orden.productos.map(productoOrden => {
                    return (
                      <p>x{productoOrden.cantidad} - { productos.get(productoOrden.idProducto).nombre }</p>  
                    );
                  })}
                </td>
                
                <td>
                  { renderDropdownEstado(orden) }
                </td>
              </FilaTabla>
            );
          })}

        </Tabla>
      );
    } else {
      return ( <p>Todavía no se han hecho órdenes de compra.</p> );
    }
  }

  return (
    <>
      <div className="stack horizontal justify-end gap-2 my-2">
        <SearchBox 
          icono="search" 
          iconoSufijo="clear"
          label="Busca órdenes por Id, nombre o email" 
          buscarEnOnChange={false}
          onBusqueda={cambiarQueryOrdenes}
        />
      </div>

      { renderTablaOrdenes() }

      <div className="mt-5">
        <ControlPaginas
          paginasTotales={paginasTotales}
          paginaInicial={paginaActual}
          onAnterior={manejarCambioPagina}
          onSiguiente={manejarCambioPagina}
        />
      </div>
    </>
  );
}