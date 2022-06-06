import React, { useState } from "react";

export default function SearchBox({ icono, iconoSufijo, label, onBusqueda }) {

    const [query, setQuery] = useState("");

    const handleBusqueda = (e) => {
        e.preventDefault();

        onBusqueda(query);
    }

    const handleCambioQuery = (e) => {
        console.log("buscando...");
        setQuery(e.target.value);
        onBusqueda(query);
    }

    const handleBorrarQuery = (e) => {

        const queryVacio = "";

        console.log("Query borrado");

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