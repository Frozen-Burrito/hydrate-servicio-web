import React, { useState } from "react";

import { validarRangoFechas } from "../../../utils/validaciones";

import { 
  SearchBox, 
  Dropdown,
  BotonIcono,
} from "../../";

export default function FiltrosParaOrdenes(props) {

  const { 
    onCambioEnFiltros 
  } = props;

  const [filtros, setFiltros] = useState({
    query: null,
    idCliente: null,
    nombreCliente: null, 
    email: null, 
    idOrden: null,
    estadoOrden: null,
    rangoFechas: {
      inicio: null,
      fin: null,
    }
  });

  const [errFechaInicial, setErrFechaInicial] = useState(null);
  const [errFechaFinal, setErrFechaFinal] = useState(null);

  const estadoDeOrden = [
    "Pendiente",
    "En Progreso",
    "Concluida",
    "Cancelada"
  ];

  /**
   * Actualiza el el query de órdenes.
   * @param {string} query El string de búsqueda del SearchBox.
   */
  function cambiarQueryOrdenes(query) {

    const queryStr = query.trim();

    if (queryStr.includes("@") && !queryStr.includes(" ")) {
      // El query es el email de un cliente.
      console.log("Filtrando por email: ", queryStr);
      setFiltros({
        ...filtros,
        idOrden: null,
        nombre: null,
        email: queryStr,
      });

    } else if (queryStr.includes("-") && !queryStr.includes(" ")) {
      // El query es el ID de una orden.
      console.log("Filtrando por ID de orden: ", queryStr);
      setFiltros({
        ...filtros,
        idOrden: queryStr,
        nombre: null,
        email: null,
      });

    } else {
      // El query es el nombre de un cliente.
      console.log("Filtrando por nombre de cliente: ", queryStr);
      setFiltros({
        ...filtros,
        idOrden: null,
        nombre: queryStr,
        email: null,
      });
    }
    
    onCambioEnFiltros(filtros);
  }

  function onChangeFiltroEstado(indiceEstadoOrden) {

    setFiltros({
      ...filtros,
      estadoOrden: indiceEstadoOrden
    });

    onCambioEnFiltros(filtros);
  }

  function onChangeFechaInicial(e) {

    const fechaIntroducida = e.target.valueAsDate;

    const fechaIso = fechaIntroducida.toISOString().substring(0, 10);

    setFiltros({
      ...filtros,
      rangoFechas: {
        ...filtros.rangoFechas,
        inicio: fechaIso,
      }
    });

    const resultadoVal = validarRangoFechas(fechaIntroducida, filtros.rangoFechas.fin);

    if (resultadoVal === null) {
      onCambioEnFiltros(filtros);
    } 
    // else {
    //   if (resultadoVal.error === ErrorDeRecurso.errFechaNoValida.error) {
    //     setErrFechaInicial("La fecha de publicación del recurso debe ser anterior a la fecha actual.");
    //   }
    // }
  }

  function onChangeFechaFinal(e) {

    const fechaIntroducida = e.target.valueAsDate;

    const fechaIso = fechaIntroducida.toISOString().substring(0, 10);

    setFiltros({
      ...filtros,
      rangoFechas: {
        ...filtros.rangoFechas,
        fin: fechaIso,
      }
    });

    const resultadoVal = validarRangoFechas(filtros.rangoFechas.inicio, fechaIntroducida);

    if (resultadoVal === null) {
      onCambioEnFiltros(filtros);
    } 
    // else {
    //   if (resultadoVal.error === ErrorDeFecha.errRangoNoCoincide.error) {
    //     setErrFechaInicial("La fecha de fin del rango debe ser posterior a la fecha de inicio.");
    //   }
    // }
  }

  const renderDropdownFiltroEstado = () => (
    <Dropdown 
      onColor="superficie"
      boton={(
        <BotonIcono 
          icono="sort"
          iconoSufijo="arrow_drop_down"
          elevacion={1}
          label={ filtros.estadoOrden != null ? estadoDeOrden[filtros.estadoOrden] : "Filtrar por estado" }
          color="fondo"
          tipo="dropdown"
        />
      )}
      items={(
        <>
          <button  
              className ="elemento-dropdown" 
              onClick={() => onChangeFiltroEstado(null)}
            >
              Ninguno
          </button>

          { estadoDeOrden.map((estado, indice) => (
            <button 
              key={indice} 
              className ="elemento-dropdown" 
              onClick={() => onChangeFiltroEstado(indice)}
            >
              { estado }
            </button>
          ))}
        </>
      )}
    />
  );

  return (
    <>      
      <div className="campo">
        <div className="campo-con-icono compacto">
          <span className="material-icons">
            today
          </span>
          <input 
            type="date" 
            name="fechaInicio" 
            className="input" 
            placeholder="Inicio" 
            // value={filtros.rangoFechas.inicio}
            onChange={e => onChangeFechaInicial(e)}/>
        </div>

        <p className="error" >
          {errFechaInicial}
        </p>
      </div>

      <div className="campo">
        <div className="campo-con-icono compacto">
          <span className="material-icons">
            date_range
          </span>
          <input 
            type="date" 
            name="fechaFin" 
            className="input" 
            placeholder="Fin" 
            // value={filtros.rangoFechas.fin}
            onChange={e => onChangeFechaFinal(e)}/>
        </div>

        <p className="error" >
          {errFechaFinal}
        </p>
      </div>

      { renderDropdownFiltroEstado() }

      <SearchBox 
        icono="search" 
        iconoSufijo="clear"
        label="Busca por ID de orden, o con nombre o email del cliente" 
        buscarEnOnChange={false}
        onBusqueda={cambiarQueryOrdenes}
      />
    </>
  );
}