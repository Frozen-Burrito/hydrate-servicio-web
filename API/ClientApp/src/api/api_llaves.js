import * as api from "./api";
import { formarTokenAuth } from "../utils/formato_token_auth";

export const fetchLlavesDeApiComoAdmin = async (numPagina = 1, jwt = "") => {

	const endpoint = "llaves";

	const resultados = await api.fetchPaginado(endpoint, numPagina, api.SIZE_PAGINA_DEFAULT, null, jwt);
	
	return resultados;
}

export const fetchStatsLlavesDeApi = async (jwt) => {
  const url = `${api.urlBase}/llaves/stats`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const fetchLlavesDeApiDelUsuario = async (jwt) => {
  const url = `${api.urlBase}/llaves/propias`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const generarLlaveDeApi = async (nombreDeLlave, jwt) => {

  const paramsUrl = new URLSearchParams({
    nombre: nombreDeLlave,
  });

  const url = `${api.urlBase}/llaves?${paramsUrl.toString()}`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.POST,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const regenerarLlave = async (idLlave, jwt) => {
	
	const url = `${api.urlBase}/llaves/${idLlave}`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const eliminarLlaveDeApi = async (idLlave, jwt, idPropietario = null) => {
  
	const paramsUrl = new URLSearchParams();

  if (idPropietario != null) paramsUrl.set("idPropietario", idPropietario);
	
	const url = `${api.urlBase}/llaves/${idLlave}${ idPropietario != null ? "?" + paramsUrl.toString() : ""}`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.DELETE,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}