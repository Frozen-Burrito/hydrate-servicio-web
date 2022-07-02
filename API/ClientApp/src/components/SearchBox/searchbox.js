import React, { useState } from "react";

/**
 * Un callback, invocado cada vez que cambia el query de búsqueda.
 * 
 * @callback onBusqueda
 * @param { string } query - El string de búsqueda.
 * @returns { void }
 */

/**
 * Un campo de búsqueda donde el usuario introduce un valor y 
 * notifica sobre cambios en el query, usando [onBusqueda].
 * @param {Object} props
 * @param {string | null} props.icono - El ícono de prefijo para el campo. 
 * @param {string | null} props.iconoSufijo - El ícono opcional al final del campo, que borra la query.
 * @param {string} props.label - El texto de placeholder para el campo.
 * @param {bool | undefined} props.buscarEnOnChange - Si es true, onBusqueda será invocado 
 * cada vez que el query del SearchBox sea modificado.
 * @param {onBusqueda} props.onBusqueda - El callback que maneja el cambio en el SearchBox.
 * @returns Un campo de búsqueda.
 */
export default function SearchBox(props) {

    const { icono, iconoSufijo, label, buscarEnOnChange, onBusqueda } = props;

    const [query, setQuery] = useState("");

    const handleBusqueda = (e) => {
        e.preventDefault();
        
        onBusqueda(query);
    }

    const handleCambioQuery = (e) => {
        const valor = e.target.value;

        setQuery(valor);

        if (buscarEnOnChange) {
            onBusqueda(valor);
        }
    }

    const handleBorrarQuery = (e) => {

        const queryVacio = "";

        setQuery(queryVacio);
        onBusqueda(queryVacio);
    }

    return (
        <form action="/" className="flex-45" onSubmit={handleBusqueda}>
                
            <div className="campo">
                <div className="campo-con-icono compacto">
                    <span className="material-icons" onClick={handleBusqueda}>
                        { icono }
                    </span>
                    <input 
                        type='text' 
                        name='query' 
                        className='input' 
                        placeholder={label}
                        value={query}
                        onChange={handleCambioQuery}
                    />
                    <span className="material-icons sufijo" onClick={handleBorrarQuery}>
                        { iconoSufijo }
                    </span>
                </div>
            </div>
        </form>
    );
}