import React from "react";
import { useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";

import { Drawer, ElementoDrawer } from "../";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

DrawerPerfil.defaultProps = {
  indiceItemActivo: 0,
};

export default function DrawerPerfil({ indiceItemActivo }) {

  const { valor: jwt, eliminarCookie: cerrarSesion } = useCookie("jwt");

  const history = useHistory();

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
      //TODO: Incluir nombre del usuario
      texto={'Usuario autenticado'}
      url={`/perfil/${idUsuario}`}
      accionFinal={(
        <button onClick={(e) => {
          e.stopPropagation();
          e.preventDefault();

          cerrarSesion();
          history.push("/");
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