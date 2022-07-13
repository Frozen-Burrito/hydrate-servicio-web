import React, { useState } from "react";

import "./list_item.css";

ListItem.defaultProps = {
	titulo: "",
	subtitulo: "",
	iconoSufijo: "",
	backgroundColor: "fondo",
	seleccionado: false,
	colapsable: false,
	children: null,
	onClick: () => {},
};

export default function ListItem(props) {
	const { 
		titulo, 
		subtitulo, 
		iconoSufijo,
		backgroundColor, 
		seleccionado,
		colapsable, 
		children, 
		onClick 
	} = props;

	const [colapsado, setColapsado] = useState(true);

	function manejarOnClick() {
		if (colapsable) {
			setColapsado(!colapsado);
		} else {
			onClick();
		}
	}

	function getIconoDeColapsable() {
		return colapsado ? "expand_more" : "expand_less";
	}

	const claseSeleccionado = seleccionado ? "seleccionado" : "";

	return (
		<>
			<button 
				type="button" 
				className={`list-item ${backgroundColor} ${claseSeleccionado} stack horizontal justify-between gap-2`}
				onClick={manejarOnClick}
			>
				<p className="titulo">{ titulo }</p>
				{ subtitulo.length > 0 ? <p className="subtitulo">{ subtitulo }</p> : null}

				<span className="material-icons sufijo">
					{ colapsable ? getIconoDeColapsable() : iconoSufijo }
				</span>
			</button>

			<div className={`contenido-list-item ${colapsado ? "colapsado" : "activo"}`}>
				{ children }
			</div>
		</>
	);
}
