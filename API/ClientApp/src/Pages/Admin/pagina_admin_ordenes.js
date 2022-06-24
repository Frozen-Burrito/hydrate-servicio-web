import React, { useState } from "react";

import useCookie from "../../utils/useCookie";
import { Layout, SearchBox, Tarjeta, DrawerAdmin } from "../../components";

export function PaginaAdminOrdenes () {

  const { valor: jwt } = useCookie('jwt');

  const [estadisticas, setEstadisticas] = useState([
    { valor: "347", label: "Órdenes completadas" },
    { valor: "5", label: "Órdenes pendientes" },
    { valor: "$2,900", label: "Ventas totales (MXN)" },
  ]);

  function filtrarOrdenes() {

  }

  return (
    <Layout>

      <DrawerAdmin />

      <div className="panel-contenido ancho-max-70">
        <h3>Órdenes de Clientes</h3>

        <div className="stack horizontal justify-center gap-2 my-3">
          { estadisticas.map((stat, indice) => (
            <Tarjeta elevacion={0} key={indice}>
              <div className="stat">
                <h1 className="mb-1" style={{ textAlign: "center" }}>{stat.valor}</h1>
                <h5 style={{ textAlign: "center" }}>{stat.label}</h5>
              </div>
            </Tarjeta>
          ))}
        </div>

        <div className="stack horizontal justify-end gap-2 my-2">
          <SearchBox 
            icono="search" 
            iconoSufijo="clear"
            label="Busca órdenes por Id, nombre o email" 
            onBusqueda={filtrarOrdenes}
          />
        </div>
      </div>
    </Layout>
  )
}