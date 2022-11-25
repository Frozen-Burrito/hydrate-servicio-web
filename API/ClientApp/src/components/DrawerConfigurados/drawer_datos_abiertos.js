import React, { useState, useEffect } from "react";
import { useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

import { Drawer, ElementoDrawer, BotonIcono } from "../";

DrawerDatosAbiertos.defaultProps = {
    mostrar: false,
    lado: "izquierda",
    indiceItemActivo: 0,
    onToggle: null,
};

const elementos = [
    { icono: "account_circle", texto: "Cantidad de agua", url: "" },
    { icono: "dashboard", texto: "Calidad de agua", url: "/calidad" },
    { icono: "forum", texto: "Resultados de metas", url: "/metas" },
    { icono: "inventory", texto: "Tipo de consumo", url: "/consumo" },
];



export default function DrawerDatosAbiertos(props) {
    
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
        url={`/datos-abiertos${item.url}`}
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
        encabezado="Datos Abiertos"
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