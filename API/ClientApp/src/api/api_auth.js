import * as api from "./api";

export const registrarUsuario = async (credenciales) => {
  const url = `${api.urlBase}/usuarios/registro`;

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify(credenciales),
    headers: new Headers({
      'Content-Type': 'application/json'
    }),
  });

  return await api.hacerPeticion(peticion);
}

export const iniciarSesion = async (credenciales) => {
  const url = `${api.urlBase}/usuarios/login`;

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify(credenciales),
    headers: new Headers({
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}