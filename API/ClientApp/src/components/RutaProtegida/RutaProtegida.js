import { useLocation, Redirect } from 'react-router-dom';
import { parseJwt, obtenerClaims } from '../../utils/parseJwt';
import useCookie from '../../utils/useCookie';

export default function RutaProtegida ({ children, rolRequerido }) {

  const { valor: token } = useCookie('jwt');
  const estaAutenticado = token !== undefined && token !== null;

  if (rolRequerido === undefined || rolRequerido === null || rolRequerido.length === 0) {
    // Si no se especifica un rol requerido, hacer que el
    // rol requerido sea administrador, por defecto.
    rolRequerido = 'ADMINISTRADOR';
  }

  // Por defecto, el rol del usuario es 'ninguno'.
  var rolDeUsuario = 'NINGUNO';

  if (estaAutenticado) {
    // Si existe un JWT, obtener el rol de usuario asociado
    // con el token.
    const datosToken = parseJwt(token);

    const claimsUsuario = obtenerClaims(datosToken);

    rolDeUsuario = claimsUsuario.rol;

    console.log(claimsUsuario);
  }
  
  const ubicacion = useLocation();

  const autorizado = estaAutenticado && rolDeUsuario === rolRequerido; 

  // Si el usuario esta autenticado y tiene el rol adecuado, 
  // mostrar la ruta a la que quiere navegar. De lo contrario, 
  // redirigir a pagina de inicio.
  return autorizado ? 
    <>
      { children }
    </> 
  : <Redirect
      replace={true}
      to='/'
      state={{ from: `${ubicacion.pathname}${ubicacion.search}`}}
    />
}