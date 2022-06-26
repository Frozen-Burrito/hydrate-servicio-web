import { formarTokenAuth } from "../utils/formato_token_auth";
import * as api from "./api";

export const fetchProductos = async (
  numPagina = 1,
  { query = null, soloDisponibles = null }, 
  jwt = "") => {

  const endpoint = "productos";

  const paramsUrl = new URLSearchParams({
    query: query,
    soloDisponibles: soloDisponibles
  });

  const resultados = await api.fetchPaginado(endpoint, numPagina, api.SIZE_PAGINA_DEFAULT, paramsUrl, jwt);

  return resultados;
}

export const fetchOrdenes = async (numPagina = 1, jwt = "", {
  query = null, 
  idCliente = null,
  nombreCliente = null, 
  email = null, 
  idOrden = null,
  estadoOrden = null,
}) => {

  const endpoint = "ordenes";

  const paramsUrl = new URLSearchParams({
    query,
    idOrden,
    idCliente,
    nombreCliente,
    emailCliente: email,
    estado: estadoOrden,
  });

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