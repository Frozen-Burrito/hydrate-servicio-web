
export const urlBase = '/api/v1'; 

export const registrarUsuario = async (credenciales) => {
  const url = `${urlBase}/usuarios/registro`;

  const peticion = new Request(url, {
    method: 'POST',
    body: JSON.stringify(credenciales),
    headers: new Headers({
      'Content-Type': 'application/json'
    }),
  });

  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}

export const iniciarSesion = async (credenciales) => {
  const url = `${urlBase}/usuarios/login`;

  const peticion = new Request(url, {
    method: 'POST',
    body: JSON.stringify(credenciales),
    headers: new Headers({
      'Content-Type': 'application/json'
    }),
  });
  
  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}

export const obtenerRecursos = async (jwt) => {

  const url = `${urlBase}/recursos`;

  const peticion = new Request(url, {
    method: 'GET',
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}

export const agregarRecurso = async (jwt, recurso) => {

  const url = `${urlBase}/recursos`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: 'POST',
    body: JSON.stringify(recurso),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}

export const editarRecurso = async (jwt, recurso) => {

  const url = `${urlBase}/recursos/${recurso.id}`;

  const peticion = new Request(url, {
    method: 'PUT',
    body: JSON.stringify(recurso),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}

export const eliminarRecurso = async (jwt, idRecurso) => {

  // La url de un recurso especifico, con idRecurso.
  const url = `${urlBase}/recursos/${idRecurso}`;

  const peticion = new Request(url, {
    method: 'DELETE',
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  const resultado = await fetch(peticion);

  const resJson = await resultado.json();

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}