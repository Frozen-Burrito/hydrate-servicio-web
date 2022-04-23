using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;

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

        public Task<List<DTOProducto>> GetProductos(bool soloDisponibles)
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