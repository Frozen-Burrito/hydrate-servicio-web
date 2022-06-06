import React, { useState } from "react";
import { Link, useHistory } from "react-router-dom";

import useCookie from "../../utils/useCookie";
import { getIdUsuarioDesdeJwt } from "../../utils/parseJwt";
import { StatusHttp } from "../../api/api";
import { 
	fetchRespuestasAComentario,
	marcarUtilComentarioConId,
	reportarComentarioConId,
	marcarUtilRespuestaConId,
	reportarRespuestaConId, 
	eliminarComentarioConId,
	eliminarRespuestaConId
} from "../../api/api_comentarios";

import { Avatar, Dropdown, BotonIcono } from "../";
import Tarjeta, { onClickAccion } from "../Tarjeta/tarjeta";

TarjetaComentario.defaultProps = {
	idComentarioPadre: -1,
	idUsuarioActual: null,
	comentario: {
		id: -1,
		asunto: "",
		contenido: "",
		fecha: "",
		publicado: true,
		idAutor: "",
		nombreAutor: "",
		numeroDeRespuestas: 0,
		numeroDeReportes: 0,
		reportadoPorUsuarioActual: false,
		numeroDeUtil: 0,
		utilParaUsuarioActual: false,
	},
	onComentarioEliminado: (idComentario) => {},
};

export default function TarjetaComentario(props) {
	// Destructurar props del componente.
	const { comentario, idComentarioPadre, idUsuarioActual, onComentarioEliminado } = props;

	const [esComentario] = useState(idComentarioPadre < 0);

	const { valor: jwt } = useCookie("jwt");

	const history = useHistory();

	// Estado del comentario.
	const [mostrandoRespuestas, setMostrandoRespuestas] = useState(false);
	const [esUtil, setEsUtil] = useState(comentario.utilParaUsuarioActual);
	const [fueReportado, setFueReportado] = useState(
		comentario.reportadoPorUsuarioActual
	);

	// Respuestas (solo si la tarjeta es un comentario y no una respuesta)
	const [respuestas, setRespuestas] = useState(null);
	const [estaCargando, setEstaCargando] = useState(false);
	const [tieneError, setTieneError] = useState(false);

	const esComentarioPropio = getIdUsuarioDesdeJwt(jwt) === comentario.idAutor;
	
	// Clases de estilos.
	const claseSizeTarjeta = !esComentario ? "ancho-max-80 alinear-margen-izq" : "";
	const claseColapsableResp = mostrandoRespuestas ? "expandido" : "colapsado";

	/** Permite al usuario escribir una respuesta al comentario. */
	const handleClickRespuestas = (e) => {
		if (esComentario) {
			history.push(`/comentarios/responder/${comentario.id}`);
		}
	};

	const desplegarRespuestas = (e) => {

		if (esComentario && comentario.numeroDeRespuestas > 0) {

			if (respuestas == null) {
				obtenerRespuestas();
			}

			setMostrandoRespuestas(!mostrandoRespuestas);
		}
	};

	/** Activa o desactiva la "marca de utilidad" del comentario para el usuario. */
	const toggleMarcaUtil = async (e) => {

		const resultado = esComentario
			? await marcarUtilComentarioConId(comentario.id, jwt)
			: await marcarUtilRespuestaConId(idComentarioPadre, comentario.id, jwt)

		if (resultado.ok && resultado.status === StatusHttp.Status204SinContenido) {
			const marcadoComoUtil = !comentario.utilParaUsuarioActual;

			console.log(
				`El comentario ${comentario.id} fue util para el usuario: ${marcadoComoUtil}`
			);

			comentario.utilParaUsuarioActual = marcadoComoUtil;
			comentario.numeroDeUtil += marcadoComoUtil ? 1 : -1;
			
			setEsUtil(marcadoComoUtil);
		} else {
			console.error(`Error marcando el comentario/respuesta ${comentario.id} como util.`);
		}
	};

	/** Activa o desactiva el estado de reporte del comentario.
	 *
	 * Si __comentario.reportadoPorUsuarioActual__ es __true__, este
	 * comentario ha sido reportado por el usuario.
	 */
	const toggleReporte = async (e) => {

		const resultado = esComentario
		? await reportarComentarioConId(comentario.id, jwt)
		: await reportarRespuestaConId(idComentarioPadre, comentario.id, jwt)

		if (resultado.ok && resultado.status === StatusHttp.Status204SinContenido) {
			const comentarioReportado = !comentario.reportadoPorUsuarioActual;

			console.log(
				`El comentario ${comentario.id} fue reportado por el usuario: ${comentarioReportado}`
			);

			comentario.reportadoPorUsuarioActual = comentarioReportado;
			comentario.numeroDeReportes += comentarioReportado ? 1 : -1;
			
			setFueReportado(comentarioReportado);
		} else {
			console.error(`Error reportando el comentario/respuesta ${comentario.id}.`);
		}
	};

	const eliminarComentario = async () => {
		setEstaCargando(true);

		// Eliminar el comentario representado por esta tarjeta.
		const resultado = esComentario
			? await eliminarComentarioConId(comentario.id, jwt)
			: await eliminarRespuestaConId(idComentarioPadre, comentario.id, jwt);

		console.log(resultado);

		if (resultado.ok && resultado.status === StatusHttp.Status204SinContenido) {
			onComentarioEliminado(comentario.id);
		} else {
			setTieneError(true);
		}

		setEstaCargando(false);
	}

	/**
	 * Intenta obtener las respuestas al comentario de la tarjeta.
	 * 
	 * Si es exitoso, respuestas se actualiza con el nuevo array de
	 * respuestas. Si no, tieneError sera true.
	 */
	const obtenerRespuestas = async () => {
		if (esComentario && comentario.numeroDeRespuestas > 0) {
			setEstaCargando(true);

			// Obtener todas las respuestas al comentario. Ya vienen ordenados por
			// fecha desde la API.
			const resultado = await fetchRespuestasAComentario(comentario.id, jwt);

			console.log(resultado);

			if (resultado.ok && resultado.status === StatusHttp.Status200OK) {
				setRespuestas(resultado.cuerpo);
			} else {
				setTieneError(true);
			}

			setEstaCargando(false);
		} else {
			throw new Error("Intentando obtener respuestas de una respuesta. Solo es posible obtener respuestas de un comentario.");
		}
	};

	const dropdownOpciones = (
    <Dropdown 
      onColor="background"
      boton={(
        <span className="material-icons">
          more_vert
        </span>
      )}
      items={(
        <>
          <Link to={`/comentarios/publicar/${comentario.id}`} className='elemento-dropdown'>
            Modificar
          </Link>

          <button className ="elemento-dropdown" onClick={eliminarComentario}>
            Eliminar
          </button>
        </>
      )}
    />
  );

	const renderAlertaArchivado = () => {
		if (comentario.publicado || comentario.numeroDeReportes > 5) {
			return (
				<p className="error">
					Este comentario ha sido archivado, por ahora. 
					<Link to="/guias-usuario/comentarios/removidos">¿Por qué fue removido mi comentario?</Link></p>
			);
		} else {
			return null;
		}
	}

	const renderRespuestas = () => {

		if (!esComentario) {
			throw new Error("Intentando mostrar respuestas de una respuesta.");
		}

		// Retornar placeholder de carga, si aún se estan cargando las
		// respuestas.
		if (estaCargando) {
			return (
				<div className="placeholder">
					<p>Cargando respuestas...</p>
				</div>
			);
		}

		// Mostrar error, si lo hay.
		if (tieneError) {
			return (
				<div className="placeholder">
					<p>Error cargando las respuestas</p>
				</div>
			);
		}

		// Si hay respuestas publicadas, mostrar la lista de respuestas.
		// Si aún no hay, mostrar un placeholder.
		if (respuestas != null && respuestas.length > 0) {

			console.log(respuestas.length);

			const contenidoRespuestas = respuestas.map((respuesta) => {
				return (
					<TarjetaComentario
						key={respuesta.id}
						comentario={respuesta}
						idComentarioPadre={comentario.id}
						idUsuarioActual={idUsuarioActual}
						onComentarioEliminado={(idRespuesta) => {
              setRespuestas(respuestas.filter(respuesta => respuesta.id !== idRespuesta))
            }}
					/>
				);
			});

			return contenidoRespuestas;
		} else {
			return (
				<div className="placeholder">
					<p>Aún no hay respuestas publicadas.</p>
				</div>
			);
		}
	};

	return (
		<div className={`contenedor-comentario ${claseSizeTarjeta} mb-2`}>
			<Tarjeta
				titulo={comentario.asunto}
				subtitulo={comentario.fecha.substring(0, 10)}
				cuerpo={comentario.contenido}
				prefijo={<Avatar alt={comentario.nombreAutor} />}
				accionPrincipal={esComentario ? desplegarRespuestas : null }
				sufijo={
					(esComentarioPropio) 
						? dropdownOpciones
						: null
				}
				acciones={
					<div className="stack horizontal justify-start gap-2">
						{esComentario && (
							<BotonIcono
								icono="forum"
								label={comentario.numeroDeRespuestas.toString()}
								tipo="texto"
								// Quizas mostrarlo azul si el usuario respondio a este comentario?
								// seleccionado={respuestasActivas}
								onClick={(e) => onClickAccion(e, handleClickRespuestas)}
							/>
						)}

						<BotonIcono
							icono="back_hand"
							label={comentario.numeroDeUtil.toString()}
							tipo="texto"
							seleccionado={esUtil}
							disabled={esComentarioPropio}
							onClick={(e) => onClickAccion(e, toggleMarcaUtil)}
						/>

						<BotonIcono
							icono="feedback"
							label={comentario.numeroDeReportes.toString()}
							tipo="texto"
							seleccionado={fueReportado}
							esDeError={true}
							disabled={esComentarioPropio}
							onClick={(e) => onClickAccion(e, toggleReporte)}
						/>

						{ renderAlertaArchivado() }
					</div>
				}
			>
				<p>{comentario.contenido}</p>
			</Tarjeta>

			<div className={`contenido-colapsable ${claseColapsableResp} mt-1`}>
				{ esComentario && renderRespuestas() }
			</div>
		</div>
	);
}
