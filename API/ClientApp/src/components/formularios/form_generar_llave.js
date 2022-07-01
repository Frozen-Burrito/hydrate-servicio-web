import React, { useState } from "react";
import { useHistory } from "react-router-dom";

import { estaVacio } from "../../utils/validaciones";

import useCookie from "../../utils/useCookie";

import {
	StatusHttp,
	resultadoEsOK,
	resultadoEsErrCliente,
	resultadoEsErrServidor,
} from "../../api/api";

import { generarLlaveDeApi } from "../../api/api_llaves";

import "./formularios.css";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";

export default function FormGenerarLlaveApi() {

	const { valor: jwt } = useCookie("jwt");

	const [valores, setValores] = useState({
		nombreLlave: "",
	});

	const [errores, setErrores] = useState({
		general: "",
		nombreLlave: "",
	});

	const [longitudesMax] = useState({
		nombreLlave: 32,
	});

	const [estaCargando, setEstaCargando] = useState(false);

	const history = useHistory();

	/**
	 * Valida y actualiza el estado de los valores y errores del
	 * formulario.
	 */
	const handleCambioValor = (e) => {
		const nombreValor = e.target.name;
		const valor = e.target.value;

		// let resultadoVal = "";

		switch (nombreValor) {
			case "nombreLlave":
				//TODO: Especificar validaciones, cuando sean necesarias.
				// resultadoVal = null;
				break;
			default:
				console.error(
					`Un valor que no existe fue modificado: [${nombreValor}]: ${valor}`
				);
				break;
		}

		setValores({
			...valores,
			[nombreValor]: valor,
		});

		let errorEnValor = "";

		// if (resultadoVal != null) {
		// 	if (resultadoVal.error === ErrorDeValidacion.correoVacio.error) {
		// 		errorEnValor = "El correo electrónico es obligatorio";
		// 	}
		// }

		setErrores({
			...errores,
			[nombreValor]: errorEnValor,
		});
	};

	async function handleObtenerNuevaLlave(e) {
		e.preventDefault();
		setEstaCargando(true);

		const { nombreLlave } = valores;

		const resultado = await generarLlaveDeApi(nombreLlave, jwt);

		if (resultado != null) {

			const { ok, status } = resultado;

			if (ok && resultadoEsOK(status)) {
				const idUsuario = getIdUsuarioDesdeJwt(jwt);

				history.push(`/perfil/${idUsuario}/llaves`);

			} else if (resultadoEsErrCliente(status)) {

				let mensaje = "El servicio no está disponible, intente más tarde.";

				if (status === StatusHttp.Status405NoPermitido)
				{
					mensaje = "Ya tienes 3 llaves de API registradas. Elimina una antes de obtener una nueva.";
				}

				setEstaCargando(false);
				setErrores({
					...errores,
					general: mensaje,
				});
			} else if (resultadoEsErrServidor(status)) {

				setEstaCargando(false);
				setErrores({
					...errores,
					general: resultado.cuerpo.mensaje,
				});
			}
		} else {

			setEstaCargando(false);
			setErrores({
				...errores,
				general: "Hubo un error inesperado.",
			});
		}
	}
	
	function renderCampoNombreLlave() {

		const longitudNombreExcede = valores.nombreLlave.length > longitudesMax.nombreLlave;

		return (
			<div className="form-group">
				<div className="campo expandir">
					<div className="campo-con-icono">
						<span className="material-icons">drive_file_rename_outline</span>
						<input
							required
							type="text"
							name="nombreLlave"
							disabled={estaCargando}
							className="input"
							placeholder="Nombre para la llave de API"
							value={valores.nombreLlave}
							onChange={handleCambioValor}
						/>
					</div>

					<div className="stack horizontal justify-between gap-2 mt-1">
						<p className="error">{errores.nombreLlave}</p>

						<p className={longitudNombreExcede ? "error" : ""}>
							{`${valores.nombreLlave.length} / ${longitudesMax.nombreLlave}`}
						</p>
					</div>
				</div>
			</div>
		);
	}

	// Es true si existen errores de validación en el formulario.
	// (Todos los campos son obligatorios).
	const tieneErrores = !estaVacio(errores.nombreLlave);

	// Desactivar el botón de enviar formulario si está cargando o tiene errores.
	const submitDesactivado = estaCargando || tieneErrores;

	return (
		<div className="form-container w-50">
			<form>
				<h2 className="center-text mt-7 mb-3">Obtener una Nueva Llave de API</h2>

				<div className="form-fields">
					{ renderCampoNombreLlave() }

					<p className="error mt-1">{errores.general}</p>
				</div>

				<div className="end-text mt-5">
					<button
						className={`btn btn-primario ${
							submitDesactivado ? "btn-desactivado" : ""
						}`}
						disabled={submitDesactivado}
						onClick={handleObtenerNuevaLlave}
					>
						Confirmar
					</button>
				</div>
			</form>
		</div>
	);
}
