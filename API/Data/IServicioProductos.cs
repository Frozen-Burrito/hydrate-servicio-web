using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data 
{
    public interface IServicioProductos
    {
        Task<List<DTOProducto>> GetProductos(bool soloDisponibles);

        Task<DTOProducto> GetProductoPorId(int idProducto);

        Task<DTOProducto> NuevoProducto(DTONuevoProducto nuevoProducto);

        Task<DTOProducto> ModificarCantidadDisponible(int idProducto, int nuevaCantidad);

        Task EliminarProducto(int idProducto);
    }
}