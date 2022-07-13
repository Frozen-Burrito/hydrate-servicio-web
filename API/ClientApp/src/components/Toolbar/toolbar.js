import React from "react";

import { BotonIcono } from "../";

import "./toolbar.css";

Toolbar.defaultProps = {
	tieneDrawer: false,
	onCerrarMenu: null,
	startChildren: null,
	endChildren: null
}

export default function Toolbar(props) {

	const {
		tieneDrawer,
		onToggleMenu,
		startChildren,
		endChildren,
	} = props;

	return (
		<>
			<nav className="toolbar mt-5">
				<div className="container">
					<div className="stack horizontal justify-between gap-1">
						{ tieneDrawer && 
							<BotonIcono 
								icono="menu"
								tipo="texto"
								iconoAlFinal={true}
								onClick={onToggleMenu}
							/>
						}

						<div>
							{ startChildren }
						</div>

						<div className="stack horizontal justify-end gap-2">
							{ endChildren }
						</div>
					</div>
				</div>
			</nav>
		</>
	);
}