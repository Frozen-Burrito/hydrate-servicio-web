import * as api from "./api";
import { getIdUsuarioDesdeJwt } from "../utils/parseJwt";

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