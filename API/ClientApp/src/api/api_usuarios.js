import * as api from "./api";

import { formarTokenAuth } from "../utils/formato_token_auth";

export const getUsernameYCorreo = async (jwt) => {

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

  const resultado = await api.hacerPeticion(peticion);

  if (api.resultadoEsOK(resultado.status)) {

    // Solo obtener nombre de usuario y email del cuerpo, nada mas.
    const { nombreUsuario, email } = resultado.cuerpo;

    return { nombreUsuario, email };
  } else {
    throw "Error obteniendo datos de usuario, status: " + resultado.status;
  }
}