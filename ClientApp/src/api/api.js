
export const urlBase = 'api/v1'; 

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