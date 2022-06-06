import React from "react";

import { getIniciales, stringAColor } from "../../utils/obtener_avatar";

import "./avatar.css";

Avatar.defaultProps = {
    url: null,
    alt: "",
};

/**
 * Recibe una URL opcional a una imagen de perfil, así como el nombre 
 * del usuario.
 * 
 * Si la URL es válida, el __Avatar__ muestra la foto de perfil del usuario.
 * Si no se usa una URL o no es válida, el __Avatar__ muestra las dos iniciales
 * del nombre del usuario, con un fondo de color semi-aleatorio.
 */
export default function Avatar(props) {

    const { url, alt } = props;

    const tieneImagen = url != null;

    if (tieneImagen) {
        return (
            <img class="avatar" src={url} alt={alt} />
        );
    } else {

        const iniciales = getIniciales(alt);
        const colorAvatar = stringAColor(alt);

        return (
            <div className="avatar avatar-color" style={{ backgroundColor: colorAvatar }}>
                <p>{iniciales}</p>
            </div>
        )
    }
}