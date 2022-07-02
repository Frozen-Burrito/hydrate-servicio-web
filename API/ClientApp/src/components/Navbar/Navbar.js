import "./Navbar.css";
import React from "react"
import { Link, useHistory } from "react-router-dom";
import useCookie from "../../utils/useCookie";
import { obtenerClaims, parseJwt, getIdUsuarioDesdeJwt } from "../../utils/parseJwt";
import Dropdown from "../Dropdown/dropdown";
import BotonRedondeado from "../Botones/BotonRedondeado";

const logDatosAuth = false;

export default function Navbar() {

  const { valor: jwt, eliminarCookie: eliminarToken } = useCookie("jwt");

  const history = useHistory();

  let rolDeUsuario = "NINGUNO";

  if (jwt !== undefined && jwt !== null) {
    const datosToken = parseJwt(jwt);

    const claimsUsuario = obtenerClaims(datosToken);

    if (logDatosAuth) {
      console.log(claimsUsuario);
    }

    rolDeUsuario = claimsUsuario.rol;
  }

  const textoRegistro = "Regístrate";
  const textoIniciarSesion = "Iniciar Sesión";
  const urlIniciarSesion = "/inicio-sesion";
  const urlRegistro = "/creacion-cuenta";

  const botonesRegistro = (
    <>
      <BotonRedondeado 
        relleno={false} 
        texto={textoIniciarSesion} 
        link={urlIniciarSesion} 
      />
      <BotonRedondeado 
        relleno={true} 
        texto={textoRegistro} 
        link={urlRegistro} 
      />
      <button className="btn-menu-mobile">
        <span className="material-icons">
          menu
        </span>
      </button>
    </>
  );

  const obtenerOpcionesSegunRol = () => {
    if (rolDeUsuario === "NINGUNO") {
      return (
        <>
          <Link to={`/perfil/${getIdUsuarioDesdeJwt(jwt) ?? ""}/llaves`} className="elemento-dropdown">
            Llaves de API
          </Link>
        </>
      );
    } else if (rolDeUsuario === "MODERADOR_COMENTARIOS") {
      return (
        <>
          <Link to="/admin/comentarios" className="elemento-dropdown">
            Comentarios Pendientes
          </Link>
        </>
      );
    } else if (rolDeUsuario === "ADMIN_ORDENES") {
      return (
        <>
          <Link to="/admin/ordenes" className="elemento-dropdown">
            Panel de Órdenes
          </Link>
        </>
      );
    } else if (rolDeUsuario === "ADMIN_RECURSOS_INF") {
      return (
        <>
          <Link to="/admin/recursos-informativos" className="elemento-dropdown">
            Recursos Informativos
          </Link>
        </>
      );
    } else if (rolDeUsuario === "ADMINISTRADOR") {
      return (
        <>
          <Link to="/admin/llaves-api" className="elemento-dropdown">
            Utilización de API
          </Link>
          <Link to="/admin/usuarios" className="elemento-dropdown">
            Usuarios
          </Link>
        </>
      );
    }
  }

  const btnDropdown = (
    <Dropdown 
      onColor="primario"
      boton={(
        <span className="material-icons">
          account_circle
        </span>
      )}
      items={(
        <>
          <Link to={`/perfil/${getIdUsuarioDesdeJwt(jwt) ?? ""}`} className="elemento-dropdown">
            Perfil
          </Link>

          {/* Insertar las opciones de menú adecuadas, según el rol. */}
          { obtenerOpcionesSegunRol(rolDeUsuario) }

          <button className ="elemento-dropdown" onClick={() => {
            // Eliminar la cookie con el JWT.
            eliminarToken();
            // Asegurar que el usuario es redirigido, para que no se 
            // mantenga en la pagina actual (a la que ya no deberia
            // tener acceso).
            history.push("/");
          }}>
            Cerrar Sesión
          </button>
        </>
      )}
    />
  );

  return (
    <nav className="navbar capa-5">
      <div className="container">
        <div className="logo-container">
          <Link className="logo contraste-primario" to="/" style={{ textDecoration: "none" }}>Hydrate</Link>
        </div>

        <ul className="nav-links">
          <li>
            <Link className="nav-link" to="/about" style={{ textDecoration: "none" }}>Sobre Nosotros</Link>
          </li>
          <li>
            <Link className="nav-link" to="/productos" style={{ textDecoration: "none" }}>Productos</Link>
          </li>
          <li>
            <Link className="nav-link" to="/guias" style={{ textDecoration: "none" }}>Guías</Link>
          </li>
          <li>
            <Link className="nav-link" to="/datos-abiertos" style={{ textDecoration: "none" }}>Datos Abiertos</Link>
          </li>
        </ul>
        
        <div className="btns-container">
          { jwt === undefined || jwt === null ? botonesRegistro : btnDropdown }
        </div>
      </div>
    </nav>
  );
}
