import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

import { Drawer, ElementoDrawer, BotonIcono } from "../";

DrawerPerfil.defaultProps = {
  mostrar: false,
  lado: "izquierda",
  indiceItemActivo: 0,
  onToggle: null,
};

const elementos = [
  { icono: "account_circle", texto: "Mi Cuenta", url: "" },
  { icono: "dashboard", texto: "Tablero", url: "/tablero" },
  { icono: "forum", texto: "Mis Comentarios", url: "/comentarios" },
  { icono: "api", texto: "Llaves de API", url: "/llaves" },
];

export default function DrawerPerfil(props) {

  const { mostrar, lado, indiceItemActivo, onToggle } = props;

  const { valor: jwt, eliminarCookie: cerrarSesion } = useCookie("jwt");

  const [idUsuario, setIdUsuario] = useState(null);

  const history = useHistory();

  function getIconoBtnCerrar() {

    if (mostrar) {
      if (lado === "izquierda") {
        return "keyboard_arrow_left";
      } else {
        return "keyboard_arrow_right";
      }
    } else {
      if (lado === "izquierda") {
        return "keyboard_arrow_right";
      } else {
        return "keyboard_arrow_left";
      }
    }
  }

  useEffect(() => {

    setIdUsuario(getIdUsuarioDesdeJwt(jwt));

  }, [jwt]);

  const elementosMenu = elementos.map((item, indice) => (
    <ElementoDrawer
      key={indice}
      icono={item.icono}
      texto={item.texto}
      url={`/perfil/${idUsuario}${item.url}`}
      seleccionado={indiceItemActivo === indice}
    />
  ));

  const btnToggleDrawer = (
    <BotonIcono
      icono={getIconoBtnCerrar()}
      color="primario"
      tipo="fill"
      onClick={() => onToggle(!mostrar)}
    />
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
      encabezado="Perfil"
      colorFondo="primario"
      lado={lado}
      mostrar={mostrar}
      conFondo
      onCerrar={() => onToggle(false)}
      elementos={elementosMenu}
      elementoFinal={elementoCuenta}
      btnCerrarExterno={btnToggleDrawer}
    />
  );
}