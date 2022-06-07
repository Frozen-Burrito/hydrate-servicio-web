import React from "react";

import useCookie from "../../utils/useCookie";

import { Drawer, ElementoDrawer } from "../";

DrawerAdmin.defaultProps = {
  indiceItemActivo: 0,
};

export default function DrawerAdmin({ indiceItemActivo }) {

  const { eliminarCookie: cerrarSesion } = useCookie('jwt');

  const elementosMenu = (
    <>
      <ElementoDrawer
        icono="account_circle"
        texto="Usuarios"
        url="/admin/usuarios"
        seleccionado={indiceItemActivo === 0}
      />
      <ElementoDrawer
        icono="dashboard"
        texto="Órdenes"
        url="/admin/ordenes"
        seleccionado={indiceItemActivo === 1}
      />
      <ElementoDrawer
        icono="forum"
        texto="Comentarios"
        url="/admin/comentarios"
        seleccionado={indiceItemActivo === 2}
      />
      <ElementoDrawer
        icono="auto_stories"
        texto="Recuros Informativos"
        url="/admin/recursos-informativos"
        seleccionado={indiceItemActivo === 3}
      />
    </>
  );

  const elementoCuenta = (
    <ElementoDrawer
      icono="account_circle"
      texto="Usuario autenticado"
      url="/perfil"
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
      encabezado="Administrar"
      elementos={elementosMenu}
      elementoFinal={elementoCuenta}
    />
  );
}