import React from "react";

export default function BotonIcono(props) {

	const { 
		icono, 
		label, 
		tipo, 
		disabled,
		color, 
		seleccionado, 
		iconoAlFinal,
		esDeError, 
		onClick 
	} = props;

	const tieneLabel = label && label.length > 0;

	const claseTipoBtn = `btn-${tipo}`;
	const claseColor = `btn-${color}`;
	const claseDisabled = disabled ? "desactivado" : "";
	const claseSeleccionado = seleccionado && !disabled ? "seleccionado" : ""; 
	const claseColorError = seleccionado && esDeError ? "error" : "";

	const handleClick = (e) => {
		if (!disabled) {
			onClick(e);
		}
	}

	const renderLabelBtn = () => (tieneLabel) 
		? ( <p className={`${claseColorError}`}>{label}</p> )
		: null; 

	const renderContenido = () => (iconoAlFinal) 
		? (
			<>
				{ renderLabelBtn() }

				<span className={`material-icons ${claseColorError}`}>
					{ icono }
				</span>
			</>
		)
		: (
			<>
				<span className={`material-icons ${claseColorError}`}>
					{ icono }
				</span>

				{ renderLabelBtn() }
			</>
		);

	return (
		<div 
			className={`btn-icono ${claseTipoBtn} ${claseColor} ${claseDisabled} ${claseSeleccionado}`} 
			onClick={handleClick}
		>
			{ renderContenido() }				
		</div>
	);
}

BotonIcono.defaultProps = {
    icono: "add",
    label: "",
    tipo: "fill",
		color: "primario",
    iconoAlFinal: false,
    esDeError: false,
    disabled: false,
    seleccionado: false,
    onClick: () => {},
};
