using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioProductos : IServicioProductos
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioProductos(IWebHostEnvironment env, ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }
        
        public async Task EliminarProducto(int idProducto)
        {
            Producto? producto = await _contexto.Productos.FindAsync(idProducto);

            if (producto is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            _contexto.Remove(producto);
            await _contexto.SaveChangesAsync();
        }

        public async Task<DTOProducto> GetProductoPorId(int idProducto)
        {
            Producto? producto = await _contexto.Productos.FindAsync(idProducto);

            if (producto is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            return producto.ComoDTO();
        }

        public async Task<ICollection<DTOProducto>> GetProductos(DTOParamsPagina? paramsPagina, bool soloDisponibles)
        {
            IQueryable<Producto> productos = _contexto.Productos.AsQueryable();

            if (soloDisponibles)
            {
                // Filtrar productos segun su disponibilidad.
                productos = productos.Where(p => p.Disponibles > 0);
            }

            bool buscarConQuery = paramsPagina is not null && !string.IsNullOrEmpty(paramsPagina.Query);

            if (buscarConQuery)
            {   
                string strQuery = paramsPagina!.Query!.Trim().ToLower();

                // Filtrar productos segun query.
                productos = productos.Where(p => p.Nombre.ToLower().Contains(strQuery));
            }
            
            productos = productos.OrderByDescending(p => p.Id);

            IQueryable<DTOProducto> dtosProductos = productos.Select(p => p.ComoDTO());

            var productosPaginados = await ListaPaginada<DTOProducto>
                .CrearAsync(dtosProductos, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return productosPaginados;
        }

        public async Task<DTOProducto> ModificarCantidadDisponible(int idProducto, int nuevaCantidad)
        {
            Producto? producto = await _contexto.Productos.FindAsync(idProducto);

            if (producto is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            producto.Disponibles = nuevaCantidad;

            _contexto.Entry(producto).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();

            return producto.ComoDTO();
        }

        public async Task<DTOProducto> NuevoProducto(DTONuevoProducto nuevoProducto)
        {
            Producto modeloProducto = nuevoProducto.ComoNuevoProducto();

            _contexto.Add(modeloProducto);
            await _contexto.SaveChangesAsync();

            return modeloProducto.ComoDTO();
        }
    }
}
#nullable disable