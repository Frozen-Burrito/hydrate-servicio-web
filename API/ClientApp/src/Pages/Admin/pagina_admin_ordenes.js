import React, { useState, useEffect } from "react";

import useCookie from "../../utils/useCookie";
import { fetchResumenDeOrdenes, fetchOrdenes } from "../../api/api_productos";

import { 
  Layout, 
  Tarjeta, 
  DrawerAdmin, 
  TablaOrdenes
} from "../../components";

export function PaginaAdminOrdenes () {

  const { valor: jwt } = useCookie('jwt');

  // Valores de estadísticas de órdenes.
  const [ordCompletadas, setOrdCompletadas] = useState(null);
  const [ordEnProgreso, setOrdEnProgreso] = useState(null);
  const [ventasTotales, setVentasTotales] = useState(null);

  const [labelsStats] = useState([
    "Órdenes completadas",
    "Órdenes en progreso",
    "Ventas totales (MXN)"
  ]);

  /**
   * Mapea la posición de la estadística con su valor.
   * 
   * @param {number} indice El índice de la estadística de ordenes
   * @returns El valor de la estadística.
   */
  function obtenerValorDeStat(indice) {
    switch (indice) {
      case 0: return ordCompletadas;
      case 1: return ordEnProgreso;
      case 2: 
        if (ventasTotales >= 1000) {
          return `$${(ventasTotales / 1000).toFixed(2)}K`;
        } else {
          return ventasTotales;
        }
      default: return null;
    }
  }

  useEffect(() => {
    
    async function obtenerEstadisticas() {

      const resultado = await fetchResumenDeOrdenes(jwt);

      if (resultado.ok && resultado.status === 200) {

        const { cuerpo: estadisticas } = resultado;

        setOrdCompletadas(estadisticas.ordenesCompletadas);
        setOrdEnProgreso(estadisticas.ordenesEnProgreso);
        setVentasTotales(estadisticas.ventasTotalesMXN);

      } else if (resultado.status >= 500) {
        console.log(resultado.cuerpo);

      } else if (resultado.status >= 400) {

        console.log(resultado.cuerpo);
      }
    }

    obtenerEstadisticas();

  }, [jwt])
  

  return (
    <Layout>

      <DrawerAdmin />

      <section className='contenedor full-page py-5'>
        <h3 className="mt-3">Órdenes de Clientes</h3>

        <div className="stack horizontal justify-center gap-2 my-3">
          { labelsStats.map((label, indice) => {

            let valor = obtenerValorDeStat(indice);

            return (
              <Tarjeta elevacion={0} key={indice}>
                <div className="stat">
                  <h1 className="mb-1" style={{ textAlign: "center" }}>
                    { valor ?? "--" }
                  </h1>
                  <h5 style={{ textAlign: "center" }}>{ label }</h5>
                </div>
              </Tarjeta>
            );
          })}
        </div>

        <TablaOrdenes />
      </section>
    </Layout>
  )
}