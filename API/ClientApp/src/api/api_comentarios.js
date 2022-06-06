import * as api from "./api";
import { getIdUsuarioDesdeJwt } from "../utils/parseJwt";

export const fetchComentariosPublicados = async (jwt = "") => {

  const url = `${api.urlBase}/comentarios`;
  
  if (jwt) {
    const idUsuario = getIdUsuarioDesdeJwt(jwt);

    console.log();

    url.concat("?" + new URLSearchParams({
      idUsuario
    }).toString());
  }

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': jwt ? `Bearer ${jwt}` : "", 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const publicarComentario = async (comentario, jwt) => {
  const url = `${api.urlBase}/comentarios`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify(comentario),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const fetchComentarioConId = async (id, jwt = "") => {

  const url = `${api.urlBase}/comentarios/${id}`;

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  url.concat("?" + new URLSearchParams({
    idUsuario
  }).toString());

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': jwt.length > 0 ? `Bearer ${jwt}` : "", 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const modificarComentario = async (comentario, jwt) => {

  const url = `${api.urlBase}/comentarios/${comentario.id}`;

  const peticion = new Request(url, {
    method: api.PUT,
    body: JSON.stringify(comentario),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const eliminarComentarioConId = async (id, jwt) => {

  const url = `${api.urlBase}/comentarios/${id}`;

  const peticion = new Request(url, {
    method: api.DELETE,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

export const fetchComentariosDeAutor = async (idAutor, jwt = "") => {

  const url = `${api.urlBase}/comentarios/autor/${idAutor}`;

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  url.concat("?" + new URLSearchParams({
    idUsuario
  }).toString());

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': jwt.length > 0 ? `Bearer ${jwt}` : "", 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const marcarUtilComentarioConId = async (id, jwt) => {

  const url = `${api.urlBase}/comentarios/${id}/util`;

  const peticion = new Request(url, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

export const reportarComentarioConId = async (id, jwt) => {

  const url = `${api.urlBase}/comentarios/${id}/reportar`;

  const peticion = new Request(url, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

// Respuestas

export const fetchRespuestasAComentario = async (idComentario, jwt = "") => {

  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas`;

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  url.concat("?" + new URLSearchParams({
    idUsuario
  }).toString());
  
  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': jwt ? `Bearer ${jwt}` : "", 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const publicarRespuestaAComentario = async (respuesta, idComentario, jwt) => {
  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify(respuesta),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const fetchRespuestaConId = async (idComentario, idRespuesta, jwt = "") => {

  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas/${idRespuesta}`;

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  url.concat("?" + new URLSearchParams({
    idUsuario
  }).toString());

  const peticion = new Request(url, {
    method: api.GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': jwt ? `Bearer ${jwt}` : "", 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const eliminarRespuestaConId = async (idComentario, idRespuesta, jwt) => {

  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas/${idRespuesta}`;

  const peticion = new Request(url, {
    method: api.DELETE,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

export const marcarUtilRespuestaConId = async (idComentario, idRespuesta, jwt) => {

  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas/${idRespuesta}/util`;

  const peticion = new Request(url, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}

export const reportarRespuestaConId = async (idComentario, idRespuesta, jwt) => {

  const url = `${api.urlBase}/comentarios/${idComentario}/respuestas/${idRespuesta}/reportar`;

  const peticion = new Request(url, {
    method: api.PATCH,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}
