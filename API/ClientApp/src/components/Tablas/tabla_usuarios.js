import React, { useState, useEffect } from "react";

import { StatusHttp } from "../../api/api";
import { 
	RolesAutorizacion,
	fetchTodosLosUsuarios,
	modificarRolDeAutorizacion,
} from "../../api/api_auth";
import useCookie from "../../utils/useCookie";

import { 
  Tabla, 
  FilaTabla,
  FiltrosParaUsuarios,
  EncabezadoColumna, 
  ControlPaginas,
  Dropdown,
  BotonIcono,
	Avatar,
} from "../";

export default function TablaUsuarios() {

  const { valor: jwt } = useCookie("jwt");

  // Colección de usuarios de la página, para ser mostrados en la tabla. 
  const [usuarios, setUsuarios] = useState([]);

	const [filtrosUsuarios, setFiltrosUsuarios] = useState({});

	// Estados de carga y error de la tabla.
	const [estaCargando, setEstaCargando] = useState(false);
	const [tieneError, setTieneError] = useState(false);

	// Control de resultados paginados.
	const [paginaActual, setPaginaActual] = useState(1);
	const [paginasTotales, setPaginasTotales] = useState(1);
	const [resultadosPorPagina, setResultadosPorPagina] = useState(1);

	function manejarCambioPagina(e, nuevaPagina) {
		if ((nuevaPagina >= 1 && nuevaPagina <= paginasTotales) || paginasTotales == null) {
			setPaginaActual(nuevaPagina);
		}
	}

	// Columnas.
	const datosColumnas = [
		{ texto: "ID", onclick: null}, 
		{ texto: "Avatar", onclick: () => {}}, 
		{ texto: "Nombre", onclick: () => {}}, 
		{ texto: "Correo Electrónico", onclick: () => {}}, 
		{ texto: "Rol de Usuario", onclick: null}, 
	];

	const rolesDeUsuario = new Map([
		[RolesAutorizacion.ninguno, "Ninguno"],
		[RolesAutorizacion.moderadorComentarios, "Moderador de Comentarios"],
		[RolesAutorizacion.adminOrdenes, "Admin. de Órdenes"],
		[RolesAutorizacion.adminRecursos, "Admin. de Recursos Inf."],
		[RolesAutorizacion.administrador, "Admin. General"],
	]);

	async function onCambioRolDeUsuario(idUsuario, nuevoRol) {

    const resultado = await modificarRolDeAutorizacion(idUsuario, nuevoRol, jwt);

    if (resultado.ok && resultado.status === StatusHttp.Status204SinContenido) {

      setUsuarios(prev => prev.map(usuario => {
        if (usuario.id === idUsuario) {
          usuario.rolDeUsuario = nuevoRol;
        }

        return usuario;
      }));
    }
  }

	useEffect(() => {

		async function obtenerUsuarios() {
			
			setEstaCargando(true);
			setTieneError(false);

      const resultado = await fetchTodosLosUsuarios(
				jwt, 
        filtrosUsuarios,
        paginaActual,
      );

      if (resultado.ok && resultado.status === 200) {

        const resultadoPaginado = resultado.datos;

        setUsuarios(resultadoPaginado.resultados);

        setPaginaActual(resultadoPaginado.paginaActual);
        setPaginasTotales(resultadoPaginado.paginasTotales);
				setResultadosPorPagina(resultadoPaginado.resultadosPorPagina);

      } else if (resultado.status >= 500) {
        console.log(resultado);
				setTieneError(true);

      } else if (resultado.status >= 400) {

        console.log(resultado);
				setTieneError(true);
      }

			setEstaCargando(false);
    }

		obtenerUsuarios();

	}, [jwt, paginaActual, filtrosUsuarios]);

	const renderDropdownRol = (idUsuario, rolDeAutorizacion) => {

		const llaves = Array.from(rolesDeUsuario.keys());
		const valores = Array.from(rolesDeUsuario.values());

		const usuarioEsAdmin = llaves[rolDeAutorizacion] === RolesAutorizacion.administrador;
		
		return (
			<Dropdown 
				onColor="superficie"
				expandir={true}
				disabled={ usuarioEsAdmin }
				boton={(
					<BotonIcono 
						tipo="dropdown"
						icono="pending_actions"
						iconoSufijo="arrow_drop_down"
						color="fondo"
						disabled={ usuarioEsAdmin }
						label={ valores[rolDeAutorizacion] }
					/>
				)}
				items={(
					<>
						{ valores.map((valorRol, i) => (
							<button 
								key={i} 
								className ="elemento-dropdown" 
								onClick={() => onCambioRolDeUsuario(idUsuario, i)}
							>
								{ valorRol }
							</button>
						))}
					</>
				)}
			/>
		);
	}

	function renderTablaUsuarios() {

    // Retornar placeholder de carga, si aún se estan cargando los 
    // usuarios.
    if (estaCargando) {
      return ( <p>Cargando usuarios...</p> );
    }
    
    // Mostrar error, si lo hay.
    if (tieneError) {
      return ( <p>Error cargando los usuarios</p>);
    }

    // Si hay usuarios, mostrarlos.
    // Si aún no hay, mostrar un placeholder.
    if (usuarios.length > 0) {
      return (
        <Tabla 
          resultadosPorPagina={resultadosPorPagina}
          columnas={(
            <>
              { datosColumnas.map((col, indice) => (
                <EncabezadoColumna
                  key={ indice }
                  texto={ col.texto }
                  onClick={ col.onclick }
                  iconoActivo={ col.onclick != null ? "" : null }
                  iconoInactivo={ col.onclick != null ? "" : null }
                />
              )) } 
            </>
          )}>
          
          { usuarios.map(usuario => {
            return (
              <FilaTabla key={usuario.id}>
                <td>{ usuario.id }</td>
                <td className={"stack horizontal justify-start gap-1"}>
									<Avatar alt={ usuario.nombreUsuario } />
								</td>
								<td>{ usuario.nombreUsuario }</td>
                <td>	
									<a href={`mailto:${usuario.email}`}>
										{ usuario.email }
									</a>
								</td> 
                <td>
									{ renderDropdownRol(usuario.id, usuario.rolDeUsuario) }	
								</td> 
              </FilaTabla>
            );
          })}

        </Tabla>
      );
    } else {
      return ( <p>Todavía no hay usuarios registrados.</p> );
    }
  }

  return (
		<>
			<FiltrosParaUsuarios onCambioEnFiltros={setFiltrosUsuarios} />

			{ renderTablaUsuarios() }

			<div className="mt-5">
        <ControlPaginas
          paginasTotales={paginasTotales}
          paginaInicial={paginaActual}
          onAnterior={manejarCambioPagina}
          onSiguiente={manejarCambioPagina}
        />
      </div>
		</>
	);	
}