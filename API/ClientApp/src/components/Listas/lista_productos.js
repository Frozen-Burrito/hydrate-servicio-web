import React, { useState, useEffect } from "react";

import { StatusHttp, SIZE_PAGINA_DEFAULT } from '../../api/api';
import { fetchProductos } from '../../api/api_productos';

import { TarjetaProducto, ControlPaginas } from "../";

export default function ListaProductos(props) {

  const { onComprarProducto } = props;

  const [productos, setProductos] = useState([]);

  // Filtros
  const [query, setQuery] = useState("");
  const [soloDisponibles, setSoloDisponibles] = useState(false);

  const [paginaActual, setPaginaActual] = useState(1);
  const [paginasTotales, setPaginasTotales] = useState(1);

  const [estaCargando, setEstaCargando] = useState(false);
  const [buscando, setBuscando] = useState(false);
  const [tieneError, setTieneError] = useState(false);

  useEffect(() => {

    async function getProductos() {

      setEstaCargando(true);

      const filtros = { 
        query: query, 
        sizePagina: SIZE_PAGINA_DEFAULT,
        soloDisponibles: soloDisponibles 
      };

      const resultado = await fetchProductos(paginaActual, filtros);

      if (resultado.status === StatusHttp.Status200OK) {

        const datosPaginados = resultado.datos;

        setProductos(datosPaginados.resultados);
        setTieneError(false);

        setPaginaActual(datosPaginados.paginaActual);
        setPaginasTotales(datosPaginados.paginasTotales);
        
      } else {
        setTieneError(true);
      }

      setEstaCargando(false);
    }

    getProductos();

  }, [paginaActual, query, soloDisponibles]);

  function renderLista() {
    // Retornar placeholder de carga, si aún se estan cargando los 
    // productos.
    if (estaCargando) {
      return ( <p>Cargando productos...</p> );
    }
    
    // Mostrar placeholder mientras busca productos.
    if (buscando) {
      return ( <p>Encontrando el producto perdido...</p>);
    }

    // Mostrar error, si lo hay.
    if (tieneError) {
      return ( <p>Error cargando los productos</p>);
    }

    // Si existen productos, mostrar la lista de productos. 
    // Si aún no hay, mostrar un placeholder.
    if (productos.length > 0) {
      if (productos.length > 0) {

        return productos.map(producto => {

          return (
            <TarjetaProducto 
              key={producto.id} 
              producto={producto}
              onComprar={onComprarProducto}
            />
          );
        });
      } else {
        return ( <p>No se encontró ningún producto para tu búsqueda.</p> );
      }
    } else {
      return ( <p>Aún no hay productos. Vuelve más tarde.</p> );
    }
  }

  return (
    <div>
      <div className="my-3 stack horizontal justify-center gap-2" >
        { renderLista() }
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