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
  } = paramsOrdenes;

  const endpoint = "ordenes";

  const paramsUrl = new URLSearchParams();

  if (query != null) paramsUrl.set("query", query);
  if (idCliente != null) paramsUrl.set("idCliente", idCliente);
  if (nombreCliente != null) paramsUrl.set("nombreCliente", nombreCliente);
  if (email != null) paramsUrl.set("emailCliente", email);
  if (idOrden != null) paramsUrl.set("idOrden", idOrden);
  if (estadoOrden != null) paramsUrl.set("estado", estadoOrden);
  
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
      'Authorization': formarTokenAuth(jwt),
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json',
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
      'Authorization': `Bearer ${jwt}`,
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json',
    }),
  });
  
  const resultado = await api.hacerPeticion(peticion);

  return {
    secretoCliente: resultado.ok ? resultado.cuerpo.clientSecret : "",
    orden: resultado.ok ? resultado.cuerpo.orden : null,
  }
}