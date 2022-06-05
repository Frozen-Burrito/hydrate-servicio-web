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

/**
 * Crea una nueva cuenta de usuario con username y password 
 * temporales, a partir de un correo electrónico.
 * 
 * __CUIDADO__: Esta función es peligrosa, por ahora. Es necesario 
 * verificar antes el correo electrónico, enviar un email para
 * que el usuario configure su contraseña y eliminar la cuenta 
 * temporal si no es confirmada a tiempo. También restringir la 
 * cuenta a solo publicar comentarios.
 */
export const registrarUsuarioTemporal = async (email) => {
  // El usuario aun no existe, deberia estar usando
  // un correo anonimo para publicar el comentario.
  // Crear cuenta anonima (luego podria confirmarse,
  // desde el correo).
  const credencialesTemporales = {
    nombreUsuario: "Usuario Anonimo",
    email: email,
    password: "PasswordTemporalInseguro", 
  };

  return await registrarUsuario(credencialesTemporales); 
}