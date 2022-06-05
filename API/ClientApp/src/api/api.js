
export const urlBase = '/api/v1'; 

export const GET = "GET";
export const POST = "POST";
export const PUT = "PUT";
export const PATCH = "PATCH";
export const DELETE = "DELETE";

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

export const hacerPeticion = async (peticion, respuestaConCuerpo = true) => {
  const resultado = await fetch(peticion);

  const resJson = respuestaConCuerpo ? await resultado.json() : {};

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
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
