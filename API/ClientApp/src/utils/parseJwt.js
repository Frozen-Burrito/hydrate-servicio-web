
// Extrae el payload de un JWT, principalmente para conocer el ID y 
// el rol de autenticacion del usuario.
export const parseJwt = (jwt) => {
  const urlBase64 = jwt.split('.')[1];
  const strBase64 = urlBase64.replace(/-/g, '+').replace(/_/g, '/');
  const payloadJson = decodeURIComponent(atob(strBase64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));

  return JSON.parse(payloadJson);
}

// Determina el idUsuario, rol de usuario y la expiracion de un JWT.
export const obtenerClaims = (tokenDecodificado) => {
  return {
    idUsuario: tokenDecodificado.id,
    rol: tokenDecodificado['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
    expiracionMillis: tokenDecodificado.exp
  }
}

/**
 * Una función de conveniencia para decodificar el ID de usuario
 * de un JWT de autenticación.
 * 
 * @param {string} jwt El token de autenticación.
 * @returns {string?} El ID del usuario, decodificado del token.
 */
export const getIdUsuarioDesdeJwt = (jwt) => {

  if (jwt == null || jwt.length === 0) {
    return null;
  }

  const datosToken = parseJwt(jwt);

  const { idUsuario } = obtenerClaims(datosToken);

  return idUsuario;
}
