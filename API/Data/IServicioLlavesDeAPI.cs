using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data
{
	public interface IServicioLlavesDeAPI 
	{
		/// <summary>
		/// Intenta encontrar una llave de API que coincida con la llave 
		/// proporcionada.
		/// </summary>
		/// <param name="llaveProporcionada">Un string que representa la llave de API buscada.</param>
		/// <returns>Si existe, la llave de API. Si no, [null].</returns>
		Task<LlaveDeApi?> BuscarLlave(string llaveProporcionada);
		
		/// <summary>
		/// Obtiene todas las llaves de API que ha registrado el usuario.
		/// </summary>
		/// <remarks>
		/// Como un usuario solo puede tener hasta 3 llaves simultáneamente, 
		/// este método solo puede retornar colecciones que tienen entre 0 y 
		/// 3 elementos.
		/// </remakrs>
		/// <param name="idUsuario">El ID del usuario que accede a sus llaves.</param>
		/// <returns>Las llaves de API registradas por el usuario.</returns>
		Task<ICollection<DTOLlaveDeAPI>> GetLlavesDeUsuario(Guid idUsuario);

		/// <summary>
		/// Obtiene todas las llaves de API registradas.
		/// </summary>
		/// <remarks>
		/// Este método debe ser usado con cuidado, ya que solo ciertos
		/// usuarios (administradores, por ejemplo) deberían poder acceder
		/// a las llaves de API. No usar con peticiones si autenticación y 
		/// autorizoción debidas.
		/// </remarks>
		/// <param name="paramsPagina">Los controles de paginación para el resultado.</param>
		/// <returns>Un resultado paginado con todas las llaves de API.</returns>
		Task<ICollection<DTOLlaveDeAPIAdmin>> GetTodasLasLlaves(DTOParamsPagina paramsPagina);

		/// <summary>
		/// Calcula estadísticas de utilización de la API en el mes pasado.
		/// </summary>
		/// <returns>Un resumen estadístico de uso de API.</returns>
		Task<DTOStatsLlavesDeApi> GetUsoDeAPI();

		/// <summary>
		/// Genera una nueva llave de API, si el usuario tiene menos de 
		/// tres llaves.
		/// </summary>
		/// <param name="idUsuario">El usuario que desea generar la nueva llave.</param>
		Task GenerarNuevaLlave(Guid idUsuario, string nombreLlave);

		/// <summary>
		/// Regenera una llave de API existente, asignándole un nuevo valor.
		/// </summary>
		/// <remarks>
		/// Este método puede interrumpir el uso de la API por clientes
		/// que sigan utilizando el valor anterior de la llave regenerada.
		/// </remarks>
		/// <param name="idLlave">El ID de la llave a regenerar.</param>
		/// <returns>Los datos actualizados de la llave de API regenerada.</returns>
		Task<DTOLlaveDeAPI> RegenerarLlave(int idLlave);

		/// <summary>
		/// Incrementa el número de peticiones realizadas de una llave de API específica.
		/// </summary>
		/// <param name="llave">La llave de API usada para la petición.</param>
		Task RegistrarPeticion(string llave);

		/// <summary>
		/// Incrementa el número de errores ocurridos en peticiones realizadas con
		/// una llave de API específica.
		/// </summary>
		/// <param name="llave">La llave de API usada para la petición.</param>
		Task RegistrarError(string llave);
		
		/// <summary>
		/// Elimina una llave de API existente.
		/// </summary>
		/// <param name="idUsuario">El ID del usuario dueño de la llave.</param>
		/// <param name="idLlave">El ID de la llave de API.</param>
		Task EliminarLlave(Guid idUsuario, int idLlave);
	}
}
#nullable disable