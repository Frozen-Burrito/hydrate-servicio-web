import React, { useState, useEffect } from "react";

import {
	SearchBox
} from "../../";

export default function FiltroDeUsuarios(props) {

	const { onCambioEnFiltros } = props;

	const [filtros, setFiltros] = useState({
		nombreEnPerfil: null
	});

	function onCambioQueryPorNombre(query) {

		setFiltros({
			...filtros,
			nombreEnPerfil: query,
		});
	}

	useEffect(() => {
		onCambioEnFiltros(filtros);
	}, [filtros, onCambioEnFiltros]);

	return (
		<div className="stack horizontal justify-end gap-1 mt-1">
			<SearchBox 
        icono="search" 
        iconoSufijo="clear"
        label="Buscar por nombre del usuario..." 
        buscarEnOnChange={false}
        onBusqueda={onCambioQueryPorNombre}
      />
		</div>
	);
}