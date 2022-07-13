import React from "react";
import { Link } from "react-router-dom";

import { Layout } from "../components";

export function Pagina404() {
    return (
			<Layout>
				<section className="contenedor full-page">
					<div className="full-page stack vertical justify-center gap-2">
						<h1 className="mt-5">404</h1>
						<h4 className="mt-3">Esta no es la página que estás buscando.</h4>

						<Link to="/" className="body-1 mt-3">Inicio</Link>
					</div>
				</section>
			</Layout>
    );
}