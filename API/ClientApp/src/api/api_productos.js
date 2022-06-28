import { saveAs } from "file-saver";

import { formarTokenAuth } from "../utils/formato_token_auth";
import * as api from "./api";

export const fetchProductos = async (
  numPagina = 1,
  filtros = { query: null, sizePagina: null, soloDisponibles: null },
  jwt = "") => {

  const { query, sizePagina, soloDisponibles } = filtros;

  const endpoint = "productos";

  const paramsUrl = new URLSearchParams();

  if (query != null) paramsUrl.set("query", query);

  if (soloDisponibles != null) paramsUrl.set("soloDisponibles", soloDisponibles);

  const resultados = await api.fetchPaginado(endpoint, numPagina, sizePagina, paramsUrl, jwt);

  return resultados;
}

/**
 * Obtiene las órdenes de compra, que cumplan con los parámetros y filtros.
 * 
 * @param {number} numPagina -El número de página de resultados a obtener.
 * @param {string} jwt -Token de autenticación del usuario.
 * @param {Object} paramsOrdenes -Parámetros de búsqueda de órdenes.
 * @param {string | null} paramsOrdenes.query -
 * @param {string | null} paramsOrdenes.idCliente
 * @param {string | null} paramsOrdenes.nombreCliente - Filtra órdenes según el nombre de cliente de este valor.
 * @param {string | null} paramsOrdenes.email - Filtra órdenes según este email.
 * @param {string | null} paramsOrdenes.idOrden - Filtra órdenes que contengan el string en su ID.
 * @param {string | null} paramsOrdenes.estadoOrden - Filtra órdenes según su estado.
 * @returns Un resultado paginado con las órdenes disponibles.
 */
export const fetchOrdenes = async (paramsOrdenes, jwt, numPagina = 1) => {

  const {
    query = null, 
    idCliente = null,
    nombreCliente = null, 
    email = null, 
    idOrden = null,
    estadoOrden = null,
    rangoFechas = {
      inicio: null,
      fin: null,
    }
  } = paramsOrdenes;

  const endpoint = "ordenes";

  const paramsUrl = new URLSearchParams();

  if (query != null) paramsUrl.set("query", query);
  if (idCliente != null) paramsUrl.set("idCliente", idCliente);

  // Filtros por atributos de orden.
  if (nombreCliente != null) paramsUrl.set("nombreCliente", nombreCliente);
  if (email != null) paramsUrl.set("emailCliente", email);
  if (idOrden != null) paramsUrl.set("idOrden", idOrden);

  // Filtros por estado de la orden.
  if (estadoOrden != null) paramsUrl.set("estado", estadoOrden);

  // Filtros por rango de fechas.
  if (rangoFechas.inicio != null) paramsUrl.set("desde", rangoFechas.inicio);
  if (rangoFechas.fin != null) paramsUrl.set("hasta", rangoFechas.fin);

  const resultados = await api.fetchPaginado(
    endpoint, numPagina, api.SIZE_PAGINA_DEFAULT, paramsUrl, jwt
  );

  return resultados;
}

export const fetchResumenDeOrdenes = async (jwt) => {

  const url = `${api.urlBase}/ordenes/stats`;

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      "Authorization": formarTokenAuth(jwt),
      // Utiliza JSON para el cuerpo.
      "Content-Type": "application/json",
    }),
  });
  
  const resultado = await api.hacerPeticion(peticion); 

  return resultado;
}

export const crearPaymentIntent = async (
  productos,
  jwt
) => {

  const url = `${api.urlBase}/ordenes/payment-intent`;

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify({ productos }),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      "Authorization": `Bearer ${jwt}`,
      // Utiliza JSON para el cuerpo.
      "Content-Type": "application/json",
    }),
  });
  
  const resultado = await api.hacerPeticion(peticion);

  return {
    secretoCliente: resultado.ok ? resultado.cuerpo.clientSecret : "",
    orden: resultado.ok ? resultado.cuerpo.orden : null,
  }
}

export const cambiarEstadoOrden = async (idOrden, indiceNuevoEstado, jwt) => {
  
  const params = new URLSearchParams({
    nuevoEstado: indiceNuevoEstado
  });

  const endpoint = `${api.urlBase}/ordenes/${idOrden}/actualizar?`.concat(params.toString());

  const peticion = new Request(endpoint, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      "Authorization": formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      "Content-Type": "application/json"
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

/**
 * Exporta todas las ordenes en un archivo con formato especifico.
 * 
 * @param {string} formato El formato del archivo exportado (como "csv").
 * @param {string} jwt El token de autenticacion del usuario.
 * @returns Descarga un archivo
 */
export const exportarOrdenesConFormato = async (formato, jwt) => {

  // Formar un string de formato (es algo como "csv", "pdf" o "xlsx").
  const strFormato = formato.replace(".", "");

  // Hacer peticion a endpoint de descarga de archivo con ordenes.
  const endpoint = `${api.urlBase}/ordenes/exportar/${strFormato}`;

  const peticion = new Request(endpoint, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      "Authorization": formarTokenAuth(jwt)
    }),
  });
  
  const respuesta = await fetch(peticion);

  // Obtener el nombre del archivo, a partir del header "content-disposition" de 
  // la respuesta, si no es encontrado, usar "ordenes.csv".
  let nombreArchivo = "ordenes.csv";

  const headerContentDisp =  respuesta.headers.get("content-disposition");

  const partesHeader = headerContentDisp.split(";");

  partesHeader.forEach(parte => {
    // Buscar el valor de "filename" en el valor del header.
    if (parte.trim().startsWith("filename=")) {
      nombreArchivo = parte.split("=")[1].replace(/\"/g, "");
    }
  });

  // Obtener contenido del archivo como un blob.
  const blob = await respuesta.blob();

  // Guardar el blob, con el nombre especificado, usando saveAs().
  saveAs(blob, nombreArchivo);

  return {
    ok: respuesta.ok,
    status: respuesta.status,
  };
}
