using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioOrdenes : IServicioOrdenes
    {
        public Task<ICollection<DTOOrden>> BuscarOrdenes(string? nombreDelUsuario, string? emailUsuario, DTOParamsPagina? paramsPagina, Guid? idOrden, EstadoOrden? estado = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DTOOrden>> GetOrdenes(DTOParamsPagina? paramsPagina, Guid? idUsuario, DateTime? fechaInicio = null, DateTime? fechaFinal = null, EstadoOrden? estado = null)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DTOOrden>> GetOrdenesDeUsuario(Guid idUsuario, DTOParamsPagina? paramsPagina, EstadoOrden? estado = null)
        {
            throw new NotImplementedException();
        }

        public Task<DTOOrden> GetOrdenPorId(Guid idOrden)
        {
            throw new NotImplementedException();
        }

        public Task<DTOOrden> ModificarEstadoDeOrden(Guid idOrden, EstadoOrden nuevoEstado)
        {
            throw new NotImplementedException();
        }

        public Task<DTOOrden> NuevaOrden(DTONuevaOrden nuevaOrden, Guid idCliente)
        {
            throw new NotImplementedException();
        }
    }
}
#nullable disable
