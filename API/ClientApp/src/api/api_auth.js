import * as api from "./api";
import { formarTokenAuth } from "../utils/formato_token_auth";

export const RolesAutorizacion = {
  ninguno: "NINGUNO",
  moderadorComentarios: "MODERADOR_COMENTARIOS",
  adminOrdenes: "ADMIN_ORDENES",
  adminRecursos: "ADMIN_RECURSOS_INF",
  administrador: "ADMINISTRADOR",
};

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
    nombreUsuario: "anonimo99",
    email: email,
    password: "Password1Temporal2Inseguro", 
  };

  return await registrarUsuario(credencialesTemporales); 
}

export const fetchTodosLosUsuarios = async (jwt, filtros, numPagina = 1) => {

  const endpoint = "usuarios";

  const paramsUrl = new URLSearchParams();

  if (filtros.nombreEnPerfil != null) paramsUrl.set("query", filtros.nombreEnPerfil);

  const resultados = await api.fetchPaginado(endpoint, numPagina, api.SIZE_PAGINA_DEFAULT, paramsUrl, jwt);

  return resultados;
}

export const modificarRolDeAutorizacion = async (idUsuario, nuevoRol, jwt) => {

  const url = `${api.urlBase}/usuarios/${idUsuario}`;

  if (jwt === undefined || jwt === null || jwt.length === 0) {
    return {
      ok: false,
      status: 401,
      cuerpo: {},
    };
  }

  const peticion = new Request(url, {
    method: api.PATCH,
    body: JSON.stringify({ nuevoRol }),
    headers: new Headers({
      // Incluir el JWT en el header de autorizacion.
      'Authorization': formarTokenAuth(jwt), 
      // Utiliza JSON para el cuerpo.
      'Content-Type': 'application/json'
    }),
  });
  
  return await api.hacerPeticion(peticion, false);
}