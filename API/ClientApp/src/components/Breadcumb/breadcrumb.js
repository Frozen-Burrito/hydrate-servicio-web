import React from "react";
import { Link } from "react-router-dom";

import "./breadcrumb.css";

BreadCrumb.defaultProps = {
	ruta: "/",
	nombreRutaBase: null,
};

/**
 * Produce un menú estilo "migas de pan", con links de navegación para
 * cada sub-sección de la ruta actual.
 * @param {Object} props 
 * @returns 
 */
export default function BreadCrumb(props) {

	const { ruta, nombreRutaBase } = props;

	const partesDeRuta = ruta.split("/").slice(1);

	return (
		<div className="breadcrumb stack horizontal justify-start gap-1">
			{ partesDeRuta.length > 0 
				? partesDeRuta.map((subRuta, i) => (
						<div key={i} className="stack horizontal justify-start gap-1">
							<Link to={partesDeRuta
								.slice(0, i+1).reduce((rutaElemento, ruta) => rutaElemento.concat(`/${ruta}`))}>
								{ subRuta }
							</Link>

							{ (i <= partesDeRuta.length -2) && <p>/</p>}
						</div>
					))
				: <Link to="/">
						{ nombreRutaBase ?? "/" }
					</Link>
			}
		</div>
	);
}