import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

import { Drawer, ElementoDrawer, BotonIcono } from "../";

DrawerAdmin.defaultProps = {
  mostrar: false,
  lado: "izquierda",
  indiceItemActivo: 0,
  onToggle: null,
};

const elementos = [
  { icono: "account_circle", texto: "Usuarios", url: "/admin/usuarios" },
  { icono: "dashboard", texto: "Órdenes", url: "/admin/ordenes" },
  { icono: "forum", texto: "Comentarios", url: "/admin/comentarios" },
  { icono: "auto_stories", texto: "Recuros Informativos", url: "/admin/recursos-informativos" },
];

export default function DrawerAdmin(props) {

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
      url={item.url}
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
      icono="account_circle"
      texto="Usuario autenticado"
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
      encabezado="Administrar"
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