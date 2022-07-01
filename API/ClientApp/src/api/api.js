import { getIdUsuarioDesdeJwt } from "../utils/parseJwt";

export const urlBase = '/api/v1'; 

export const GET = "GET";
export const POST = "POST";
export const PUT = "PUT";
export const PATCH = "PATCH";
export const DELETE = "DELETE";

export const SIZE_PAGINA_DEFAULT = 10;

export const StatusHttp = {
  // Satisfactorias
  Status200OK: 200,
  Status201Creado: 201,
  Status204SinContenido: 204,
  // Errores de cliente
  Status400MalaPeticion: 400,
  Status401NoAutenticado: 401,
  Status403Prohibido: 403,
  Status404NoEncontrado: 404,
  Status405NoPermitido: 405,
  // Errores de servidor
  Status500ErrInterno: 500,
  Status501NoImplementado: 501,
  Status503ServicioNoDisponible: 503,
};

/**
 * Realiza una petición a la API.
 * 
 * @param {Request} peticion La petición a realizar.
 * @param {bool} respuestaConCuerpo Si la respuesta a la petición 
 * tendrá cuerpo JSON o no.
 * @returns La respuesta a la petición, incluyendo status y cuerpo.
 */
export const hacerPeticion = async (peticion) => {
  const respuesta = await fetch(peticion);

  // console.log("La respuesta tiene un body: ", respuesta.body != null);

  const datosJson = respuesta.body != null && respuesta.status !== 204
    ? await respuesta.json() 
    : null;
    
  return {
    ok: respuesta.ok,
    status: respuesta.status,
    cuerpo: datosJson,
  };
}

/**
 * Hace una petición GET a un endpoint que produce un resultado con paginación.
 * 
 * @param {string} endpoint El endpoint que realiza la acción.
 * @param {number} numPagina El número de la página.
 * @param {number} elemsPorPagina El número de elementos por página.
 * @param {string} jwt El token de autenticación del usuario.
 * @returns Una respuesta con los resultados paginados, o un error.
 */
export async function fetchPaginado(endpoint, numPagina = 1, elemsPorPagina = 25, paramsExtra = null, jwt = "", incluirIdUsuario = false) {

  const params = new URLSearchParams({
    pagina: numPagina,
  });

  if (elemsPorPagina != null) params.set("sizePagina", elemsPorPagina);
  
  if (incluirIdUsuario && jwt) {
    const idUsuario = getIdUsuarioDesdeJwt(jwt);

    params.set("idUsuarioActual", idUsuario);
  }
  
  if (paramsExtra != null && typeof paramsExtra === "object") {
    for (let [llave, valor] of paramsExtra.entries()) {
      params.set(llave, valor);
    }
  }

  const urlConParams = `${urlBase}/${endpoint}?`.concat(params.toString());

  const peticion = new Request(urlConParams, {
    method: GET,
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': `Bearer ${jwt}`, 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });

  const respuesta = await hacerPeticion(peticion, true);

  return {
    ok: respuesta.ok,
    status: respuesta.status,
    datos: respuesta.cuerpo,
  };
}

export const resultadoEsInfo = (codigoHttp) => {
  return codigoHttp >= 100 && codigoHttp < 200;
}

export const resultadoEsOK = (codigoHttp) => {
  return codigoHttp >= 200 && codigoHttp < 300;
}

export const resultadoEsRedireccion = (codigoHttp) => {
  return codigoHttp >= 300 && codigoHttp < 400;
}

export const resultadoEsErrCliente = (codigoHttp) => {
  return codigoHttp >= 400 && codigoHttp < 500;
}

export const resultadoEsErrServidor = (codigoHttp) => {
  return codigoHttp >= 500;
}
