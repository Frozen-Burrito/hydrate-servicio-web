using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data
{
    public class RepositorioOrdenes : IServicioOrdenes
    {
        public Task<List<DTOOrden>> BuscarOrdenes(string nombreDelUsuario, string emailUsuario, Guid? idOrden, EstadoOrden? estado = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<DTOOrden>> GetOrdenes(Guid? idUsuario, DateTime? fechaInicio = null, DateTime? fechaFinal = null, EstadoOrden? estado = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<DTOOrden>> GetOrdenesDeUsuario(Guid idUsuario, EstadoOrden? estado = null)
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