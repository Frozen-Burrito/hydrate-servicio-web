import React, { useState, useEffect } from "react";

import useCookie from "../../utils/useCookie";
import { fetchStatsLlavesDeApi } from "../../api/api_llaves";

import { 
  Layout, 
  Tarjeta, 
  DrawerAdmin, 
  TablaLlaves
} from "../../components";

export function PaginaAdminLlaves () {

  const { valor: jwt } = useCookie('jwt');

  const [drawerVisible, setDrawerVisible] = useState(false);

  // Valores de estadísticas de uso de la API.
  const [peticionesTotales, setPeticionesTotales] = useState(null);
  const [erroresTotales, setErroresTotales] = useState(null);
  const [llavesActivas, setLlavesActivas] = useState(null);

  const [labelsStats] = useState([
    "Peticiones Totales",
    "Resultados de Error",
    "Clientes de API Activos"
  ]);

	/**
	 * Formatea un número para que use menos caracteres, removiendo
	 * posiciones decimales y abreviando por millares.
   * 
	 * @param {number} numero El numero a formatear.
   * @returns El número en un string con formato correcto, o null si numero es nulo.
	*/
  function formatoNumero(numero) {

    if (numero == null) return null;

		if (numero >= 1000) {
			return `${(numero / 1000).toFixed(2)}K`;
		} else {
			return numero;
		}
  }

  /**
   * Mapea la posición de la estadística con su valor.
   * 
   * @param {number} indice El índice de la estadística de ordenes
   * @returns El valor de la estadística.
   */
  function obtenerValorDeStat(indice) {
    switch (indice) {
      case 0: return formatoNumero(peticionesTotales);
      case 1: return formatoNumero(erroresTotales);
      case 2: return formatoNumero(llavesActivas);
      default: return null;
    }
  }

  useEffect(() => {
    
    async function fetchEstadisticas() {

      const resultado = await fetchStatsLlavesDeApi(jwt);

      if (resultado.ok && resultado.status === 200) {

        const { cuerpo: estadisticas } = resultado;

        setPeticionesTotales(estadisticas.peticionesTotales);
        setErroresTotales(estadisticas.erroresTotales);
        setLlavesActivas(estadisticas.numDeLlavesActivas);

      } else if (resultado.status >= 500) {
        console.log(resultado.cuerpo);

      } else if (resultado.status >= 400) {

        console.log(resultado.cuerpo);
      }
    }

    fetchEstadisticas();

  }, [jwt])

  /**
   * Muestra o esconde el menú drawer.
   * 
   * @param {bool} mostrar Si el drawer debe ser visible o no.
   */
  function manejarToggleDrawer(mostrar) {
    setDrawerVisible(mostrar);
  }

  return (
    <Layout>

      <DrawerAdmin 
        lado="izquierda"
        mostrar={drawerVisible} 
        indiceItemActivo={4}
        onToggle={manejarToggleDrawer}
      />

      <section className='contenedor full-page py-5'>
        <h3 className="mt-3">Utilización de API</h3>

				<h4 className="mt-1">Durante el mes pasado.</h4>

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

        <TablaLlaves esDeAdmin={true} />
      </section>
    </Layout>
  )
}