import * as api from "./api";
import { getIdPerfilDesdeJwt } from "../utils/parseJwt";
import { formarTokenAuth } from "../utils/formato_token_auth";

export const getPerfil = async(jwt) => {

  const idPerfil = getIdPerfilDesdeJwt(jwt);
  const url = `${api.urlBase}/perfiles/${idPerfil}`;

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
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
};

export const getInformacionPerfil = async(jwt) => {

  const url = `${api.urlBase}/usuarios/datos`;
  const bearerToken = formarTokenAuth(jwt);

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': bearerToken, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
};

export const updatePerfil = async(infoPerfil, jwt) => {

  const idPerfil = getIdPerfilDesdeJwt(jwt);
  const url = `${api.urlBase}/perfiles/${idPerfil}`;

  const peticion = new Request(url, {
    method: api.PATCH,
    body: JSON.stringify(infoPerfil),
    headers: new Headers({
      'Authorization': `Bearer ${jwt}`, 
      'Content-Type': 'application/json'
    }),
  });

  return await api.hacerPeticion(peticion);
};