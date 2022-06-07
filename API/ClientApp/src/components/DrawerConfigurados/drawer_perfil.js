import React from "react";

import useCookie from "../../utils/useCookie";

import { Drawer, ElementoDrawer } from "../";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

DrawerPerfil.defaultProps = {
  indiceItemActivo: 0,
};

export default function DrawerPerfil({ indiceItemActivo }) {

  const { valor: jwt, eliminarCookie: cerrarSesion } = useCookie("jwt");

  const idUsuario = getIdUsuarioDesdeJwt(jwt);

  const elementosMenu = (
    <>
      <ElementoDrawer
        icono='account_circle'
        texto="Mi Cuenta"
        url={`/perfil/${idUsuario}`}
        seleccionado={indiceItemActivo === 0}
      />
      <ElementoDrawer
        icono='dashboard'
        texto="Tablero"
        url={`/perfil/${idUsuario}/tablero`}
        seleccionado={indiceItemActivo === 1}
      />
      <ElementoDrawer
        icono='forum'
        texto="Mis Comentarios"
        url={`/perfil/${idUsuario}/comentarios`}
        seleccionado={indiceItemActivo === 2}
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