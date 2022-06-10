using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data 
{
    public interface IServicioProductos
    {
        Task<ICollection<DTOProducto>> GetProductos(DTOParamsPagina? paramsPagina, bool soloDisponibles = true);

        Task<DTOProducto> GetProductoPorId(int idProducto);

        Task<DTOProducto> NuevoProducto(DTONuevoProducto nuevoProducto);

        Task<DTOProducto> ModificarCantidadDisponible(int idProducto, int nuevaCantidad);

        Task EliminarProducto(int idProducto);
    }
}
#nullable disable