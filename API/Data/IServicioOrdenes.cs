using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data 
{
    public interface IServicioOrdenes
    {
        /// <summary>
        /// Obtiene una lista con todas las órdenes disponibles que cumplan con
        /// los filtros recibidos.
        /// </summary>
        /// <remarks>
        /// Si el usuario es admin, idUsuario será nulo.  
        /// Si idUsuario no es nulo, solo retornar ordenes del propio usuario (no de otros usuarios).
        /// 
        /// Si fechaDesde es nulo, el rango de fechas abarca todas las órdenes antes de fechaHasta.
        /// Si fechaHasta es nulo, el rango de fechas abarca todas las órdenes después de fechaDesde.
        /// Si ambas fechas son nulas, no aplicar filtro por fecha.
        /// 
        /// Si estado no es nulo, solo retornar las órdenes que estén en el estado recibido.
        /// </remarks>
        /// <param name="idUsuario">El Id del usuario que hace la petición.</param>
        /// <param name="fechaDesde">La fecha de inicio del rango del filtro por fecha.</param>
        /// <param name="fechaHasta">La fecha de término del rango del filtro por fecha.</param>
        /// <param name="estado">El estado que debe tener una orden para ser retornada.</param>
        /// <returns>La lista de órdenes filtradas, en forma de DTOs.</returns>
        Task<ICollection<DTOOrden>> GetOrdenes(DTOParamsPagina? paramsPagina, Guid? idUsuario, DateTime? fechaDesde = null, DateTime? fechaHasta = null, EstadoOrden? estado = null);

        /// <summary>
        /// Obtiene de la BD una lista con todas las órdenes de un usuario específico.
        /// </summary>
        /// <param name="idUsuario">El Id del usuario.</param>
        /// <param name="estado">El estado que deben tener las órdenes retornadas.</param>
        /// <returns>Una lista de órdenes del usuario.</returns>
        Task<ICollection<DTOOrden>> GetOrdenesDeUsuario(Guid idUsuario, DTOParamsPagina? paramsPagina, EstadoOrden? estado = null);
        
        /// <summary>
        /// Obtiene una lista con todas las órdenes que cumplan con los 
        /// criterios de búsqueda.
        /// </summary>
        /// <remarks>
        /// En general, solo uno de los criterios y el estado estarán presentes. 
        /// (el usuario introduce un nombre, o un email, o el Id de una orden).
        /// </remarks>
        /// <param name="nombreDelUsuario">El nombre con o sin apellido del usuario.</param>
        /// <param name="emailUsuario">El correo electrónico del usuario.</param>
        /// <param name="idOrden">El identificador de la orden, opcional.</param>
        /// <param name="estado">El estado de las órdenes, opcional.</param>
        /// <returns>Una lista de órdenes resultantes.</returns>
        Task<ICollection<DTOOrden>> BuscarOrdenes(string? nombreDelUsuario, string? emailUsuario, DTOParamsPagina? paramsPagina, Guid? idOrden, EstadoOrden? estado = null);

        /// <summary>
        /// Busca una orden con un Id determinado.
        /// </summary>
        /// <remarks>
        /// Lanza un ArgumentException si no existe una orden con el idOrden
        /// recibido.
        /// </remarks>
        /// <param name="idOrden">El Id de la orden.</param>
        /// <returns>La orden con </returns>
        Task<DTOOrden> GetOrdenPorId(Guid idOrden);

        /// <summary>
        /// Registra una nueva orden.
        /// </summary>
        /// <param name="nuevaOrden">La información de cantidades y productos de la orden.</param>
        /// <param name="idCliente">El Id del usuario que realiza la orden.</param>
        /// <returns>Los datos de la orden creada.</returns>
        Task<DTOOrden> NuevaOrden(DTONuevaOrden nuevaOrden, Guid idCliente);

        /// <summary>
        /// Cambia el estado de una orden existente.
        /// </summary>
        /// <remarks>
        /// El estado de una orden solo puede ser actualizado en cierto orden:
        /// PENDIENTE -> EN_PROGRESO -> CONCLUIDA O ERROR.
        /// </remarks>
        /// <param name="idOrden">El Id de la orden.</param>
        /// <param name="nuevoEstado">El nuevo estado para la orden.</param>
        /// <returns>Los datos actualizados de la orden.</returns>
        Task<DTOOrden> ModificarEstadoDeOrden(Guid idOrden, EstadoOrden nuevoEstado);
    }
}
#nullable disable
