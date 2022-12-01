import * as api from "./api";

export const fetchRecursos = async (numPagina = 1, jwt = "") => {

  const endpoint = "recursos";

  const resultados = await api.fetchPaginado(endpoint, numPagina, api.SIZE_PAGINA_DEFAULT, null, jwt);
  
  return resultados;
}

export const agregarRecurso = async (jwt, recurso) => {

  const url = `${api.urlBase}/recursos`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.POST,
    body: JSON.stringify(recurso),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const editarRecurso = async (jwt, recurso) => {

  const url = `${api.urlBase}/recursos/${recurso.id}`;

  const peticion = new Request(url, {
    method: api.PUT,
    body: JSON.stringify(recurso),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}

export const eliminarRecurso = async (jwt, idRecurso) => {

  // La url de un recurso especifico, con idRecurso.
  const url = `${api.urlBase}/recursos/${idRecurso}`;

  const peticion = new Request(url, {
    method: api.DELETE,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion);
}