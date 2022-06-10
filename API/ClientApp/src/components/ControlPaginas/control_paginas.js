import React, { useState } from "react";

import { BotonIcono } from "../";

export default function ControlPaginas(props) {

  const { 
    paginasTotales, 
    paginaInicial,
    onAnterior,
    onSiguiente,
  } = props;

  const [paginaActual, setPaginaActual] = useState(paginaInicial);

  const tieneAnterior = paginaActual > 1;
  const tieneSiguiente = paginaActual < paginasTotales;

  const handleCambioPagina = (e, nuevaPagina) => {

    nuevaPagina = Math.min(Math.max(parseInt(nuevaPagina), 1), paginasTotales);

    if (nuevaPagina < paginaActual) {
      onAnterior(e, nuevaPagina);
    } else {
      onSiguiente(e, nuevaPagina);
    }

    setPaginaActual(nuevaPagina);
  }

  return (
    <div className="stack horizontal justify-center gap-2">
      <BotonIcono 
        icono="arrow_back"
        label="Anterior"
        tipo="texto"
        disabled={!tieneAnterior}
        onClick={(e) => handleCambioPagina(e, paginaActual -1)}
      />

      <p>Página {paginaActual} de {paginasTotales}</p>

      <BotonIcono 
        icono="arrow_forward"
        label="Siguiente"
        tipo="texto"
        iconoAlFinal={true}
        disabled={!tieneSiguiente}
        onClick={(e) => handleCambioPagina(e, paginaActual +1)}
      />
    </div>
  );
}