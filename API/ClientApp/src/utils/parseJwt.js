
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