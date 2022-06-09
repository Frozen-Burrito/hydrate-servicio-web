using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioProductos : IServicioProductos
    {
        public Task EliminarProducto(int idProducto)
        {
            throw new System.NotImplementedException();
        }

        public Task<DTOProducto> GetProductoPorId(int idProducto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ICollection<DTOProducto>> GetProductos(DTOParamsPagina? paramsPagina, bool soloDisponibles)
        {
            throw new System.NotImplementedException();
        }

        public Task<DTOProducto> ModificarCantidadDisponible(int idProducto, int nuevaCantidad)
        {
            throw new System.NotImplementedException();
        }

        public Task<DTOProducto> NuevoProducto(DTONuevoProducto nuevoProducto)
        {
            throw new System.NotImplementedException();
        }
    }
}
#nullable disable