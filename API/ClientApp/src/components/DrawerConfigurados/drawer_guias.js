import React from "react";

import { Drawer } from "../";

DrawerGuias.defaultProps = {
  mostrar: false,
  lado: "izquierda",
  onToggle: null,
  guias: null,
};

export default function DrawerGuias(props) {

  const { mostrar, lado, onToggle, guias } = props;

  return (
    <Drawer
      encabezado="Guías de Usuario"
      colorFondo="fondo"
      conFondo
      conNavbar={true}
      lado={lado}
      mostrar={mostrar}
      capa={3}
      conPadding={false}
      onCerrar={() => onToggle(false)}
      elementos={guias}
    />
  );
}