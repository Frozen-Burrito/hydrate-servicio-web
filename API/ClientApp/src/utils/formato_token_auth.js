
/**
 * Formatea un JWT como un token de autenticación "Bearer",
 * usado por la API interna.
 * 
 * @param {string} jwt Un Json Web Token.
 * @returns Un JWT con formato "Bearer".
 */
export const formarTokenAuth = (jwt) => {
  return `Bearer ${jwt}`;
}