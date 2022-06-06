import React from "react";

import useCookie from "../../utils/useCookie";

import { Drawer, ElementoDrawer } from "../";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

export default function DrawerPerfil() {

  const { valor: jwt, eliminarCookie: cerrarSesion } = useCookie("jwt");

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  const elementosMenu = (
    <>
      <ElementoDrawer
        icono='account_circle'
        texto="Mi Cuenta"
        url={`/perfil/${idUsuario}`}
      />
      <ElementoDrawer
        icono='dashboard'
        texto="Tablero"
        url={`/perfil/${idUsuario}/tablero`}
      />
      <ElementoDrawer
        icono='forum'
        texto="Mis Comentarios"
        url={`/perfil/${idUsuario}/comentarios`}
      />
    </>
  );

  const elementoCuenta = (
    <ElementoDrawer
      icono='account_circle'
      texto={'Usuario autenticado'}
      url='/perfil'
      accionFinal={(
        <button onClick={() => {
            
          cerrarSesion();
        }}>
          <span className="material-icons">
            logout
          </span>
        </button>
      )}
    />
  );

  return (
    <Drawer
      encabezado='Perfil'
      elementos={elementosMenu}
      elementoFinal={elementoCuenta}
    />
  );
}