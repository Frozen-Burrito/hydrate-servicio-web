import React, { useState } from "react";

ListItem.defaultProps = {
    titulo: "",
    subtitulo: "",
    children: null,
    iconoSufijo: "",
    expandible: false,
    onClick: () => {},
};

export default function ListItem(props) {

    const { titulo, subtitulo, children, iconoSufijo, expandible, onClick } = props;

    const [expandido, setExpandido] = useState(false);

    const renderIconoSufijo = () => {
        let nombreIcono = iconoSufijo;

        if (expandible) {
            nombreIcono = expandido ? "expand_less" : "expand_more";
        }

        return (
            <span className="material-icons sufijo">
                { nombreIcono }
            </span>
        );
    }

    return (
        <button type="button" class="list-item item-expandible">
            <p className="titulo">{titulo}</p>
            { subtitulo.length > 0 ? <p className="subtitulo">{subtitulo}</p> : null }
            
            { renderIconoSufijo() }
        </button>
    );
}