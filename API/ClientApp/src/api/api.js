
export const urlBase = '/api/v1'; 

export const GET = "GET";
export const POST = "POST";
export const PUT = "PUT";
export const PATCH = "PATCH";
export const DELETE = "DELETE";

export const hacerPeticion = async (peticion, respuestaConCuerpo = true) => {
  const resultado = await fetch(peticion);

  const resJson = respuestaConCuerpo ? await resultado.json() : {};

  return {
    ok: resultado.ok,
    status: resultado.status,
    cuerpo: resJson,
  };
}
