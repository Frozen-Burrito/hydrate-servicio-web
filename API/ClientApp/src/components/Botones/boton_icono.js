import React from "react";

export default function BotonIcono(props) {

    const { icono, label, tipo, disabled, seleccionado, esDeError, onClick } = props;

    const tieneLabel = label && label.length > 0;

    const claseTipoBtn = `btn-${tipo}`;
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

    return (
        <div 
            className={`btn-icono ${claseTipoBtn} ${claseDisabled} ${claseSeleccionado}`} 
            onClick={handleClick}
        >
            <span className={`material-icons ${claseColorError}`}>
                { icono }
            </span>

            { renderLabelBtn() }
        </div>
    );
}

BotonIcono.defaultProps = {
    icono: "add",
    label: "",
    tipo: "fill",
    esDeError: false,
    disabled: false,
    seleccionado: false,
    onClick: () => {},
};
