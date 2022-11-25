using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Data
{
    public class RepositorioOrdenes : IServicioOrdenes
    {
        private readonly ContextoDBMysql _contexto;

        public RepositorioOrdenes(IWebHostEnvironment env, ContextoDBMysql contexto)
        {
            this._contexto = contexto;
        }

        public async Task<ICollection<DTOOrden>> GetOrdenes(DTOParamsPagina? paramsPagina, DTOParamsOrden paramsOrden)
        {
            if (await _contexto.Ordenes.CountAsync() <= 0)
            {
                // Si no existe ningun comentario, retornar una lista vacia desde el principio.
                return new List<DTOOrden>();
            }

            IQueryable<Orden> ordenes = _contexto.Ordenes
                .Include(o => o.Cliente)
                .ThenInclude(c => c.PerfilDeUsuario)
                .AsQueryable();

            if (paramsOrden.IdCliente is not null && ordenes.Count() > 0)
            {
                ordenes = ordenes.Where(o => o.Cliente.Id.Equals(paramsOrden.IdCliente));
            }

            if (paramsOrden.Estado is not null && ordenes.Count() > 0) 
            {
                ordenes = ordenes.Where(o => o.Estado.Equals(paramsOrden.Estado));
            }

            if (paramsOrden.EmailCliente is not null && ordenes.Count() > 0)
            {
                ordenes = ordenes.Where(o => o.Cliente.Email.Contains(paramsOrden.EmailCliente));
            }

            if (paramsOrden.NombreCliente is not null && ordenes.Count() > 0)
            {
                //TODO: usar nombre asociado al perfil.
                ordenes = ordenes.Where(o => 
                    o.Cliente.PerfilDeUsuario.NombreCompleto
                        .Contains(paramsOrden.NombreCliente)
                );
            }

            if (paramsOrden.IdOrden is not null && ordenes.Count() > 0)
            {
                ordenes = ordenes.Where(o => o.Id.Equals(paramsOrden.IdOrden));
            }

            if (paramsOrden.Desde is not null)
            {
                ordenes = ordenes.Where(o => o.Fecha >= paramsOrden.Desde);
            }

            if (paramsOrden.Hasta is not null)
            {
                ordenes = ordenes.Where(o => o.Fecha <= paramsOrden.Hasta);
            }

            ordenes = ordenes                
                .Include(o => o.Cliente)
                .ThenInclude(c => c.PerfilDeUsuario)
                .Include(o => o.Productos)
                .ThenInclude(o => o.Producto)
                .AsSplitQuery()
                .OrderByDescending(o => o.Fecha);
                
            IQueryable<DTOOrden> dtosOrdenes = ordenes.Select(o => o.ComoDTO());

            var ordenesPaginadas = await ListaPaginada<DTOOrden>
                .CrearAsync(dtosOrdenes, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return ordenesPaginadas;
        }

        public async Task<IEnumerable<DTOOrden>> ExportarTodasLasOrdenes()
        {
            if (await _contexto.Ordenes.CountAsync() <= 0)
            {
                return new List<DTOOrden>();
            }

            IEnumerable<DTOOrden> ordenes = await _contexto.Ordenes
                .Include(o => o.Cliente)
                .ThenInclude(c => c.PerfilDeUsuario)
                .Include(o => o.Productos)
                .ThenInclude(o => o.Producto)
                .AsSplitQuery()
                .OrderByDescending(o => o.Fecha)
                .Select(o => o.ComoDTO())
                .ToListAsync();

            return ordenes;
        }

        public async Task<DTOOrden> GetOrdenPorId(Guid idOrden)
        {
            if (await _contexto.Ordenes.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe una orden con el ID especificado");
            }

            // Orden? orden = await _contexto.Ordenes.FindAsync(idOrden);
            Orden? orden = await _contexto.Ordenes
                .Where(o => o.Id.Equals(idOrden))
                .Include(o => o.Cliente)
                .ThenInclude(c => c.PerfilDeUsuario)
                .Include(o => o.Productos)
                .FirstOrDefaultAsync();

            if (orden is null) 
            {
                throw new ArgumentException("No existe una orden con el ID especificado");
            }

            return orden.ComoDTO();
        }

        public async Task<DTOStatsOrdenes> GetStatsOrdenes()
        {
            if (await _contexto.Ordenes.CountAsync() <= 0) 
            {
                return new DTOStatsOrdenes();
            }

            IQueryable<Orden> todasLasOrdenes = _contexto.Ordenes.AsQueryable();

            int numOrdenesCompletadas = await todasLasOrdenes
                .Where(o => o.Estado == EstadoOrden.CONCLUIDA)
                .CountAsync();

            int numOrdenesEnProgreso = await todasLasOrdenes
                .Where(o => o.Estado.Equals(EstadoOrden.EN_PROGRESO))
                .CountAsync();

            List<DTOOrden> dtosOrdenes = await todasLasOrdenes
                .Include(o => o.Cliente)
                .Include(o => o.Productos)
                .ThenInclude(o => o.Producto)
                .Select(o => o.ComoDTO())
                .ToListAsync();

            decimal ventasTotales = dtosOrdenes
                .Sum(oDTO => oDTO.MontoTotal);
            
            return new DTOStatsOrdenes
            {
                OrdenesCompletadas = numOrdenesCompletadas,
                OrdenesEnProgreso = numOrdenesEnProgreso,
                VentasTotalesMXN = ventasTotales,
            };
        }

        public async Task<DTOOrden> ModificarEstadoDeOrden(Guid idOrden, EstadoOrden nuevoEstado)
        {
            if (await _contexto.Ordenes.CountAsync() <= 0) 
            {
                throw new ArgumentException("No existe una orden con el ID especificado");
            }

            // Orden? orden = await _contexto.Ordenes.FindAsync(idOrden);
            Orden? orden = await _contexto.Ordenes
                .Where(o => o.Id.Equals(idOrden))
                .Include(o => o.Cliente)
                .ThenInclude(c => c.PerfilDeUsuario)
                .Include(o => o.Productos)
                .ThenInclude(po => po.Producto)
                .FirstOrDefaultAsync();

            if (orden is null) 
            {
                throw new ArgumentException("No existe una orden con el ID especificado");
            } else 
            {
                if (orden.Estado == EstadoOrden.CONCLUIDA || orden.Estado == EstadoOrden.CANCELADA)
                {
                    throw new InvalidOperationException("Intentando cambiar el estado de una orden concluida o cancelada");
                }

                orden.Estado = nuevoEstado;

                _contexto.Entry(orden).State = EntityState.Modified;
                await _contexto.SaveChangesAsync();

                return orden.ComoDTO();
            }
        }

        public async Task<DTOOrden> NuevaOrden(DTONuevaOrden nuevaOrden, Guid idCliente)
        {
            Usuario? cliente = await _contexto.Usuarios
                .Where(u => u.Id.Equals(idCliente))
                .Include(u => u.PerfilDeUsuario)
                .FirstOrDefaultAsync();

            if (cliente is null) 
            {
                throw new ArgumentException("El usuario cliente no es válido, imposible crear orden.");
            }

            IEnumerable<int> idsProductosOrdenados = nuevaOrden.Productos
                .Select(p => p.IdProducto);

            var productosExistentes = await _contexto.Productos
                .Where(p => idsProductosOrdenados.Contains(p.Id))
                .ToListAsync();

            if (productosExistentes.Count != idsProductosOrdenados.Count())
            {
                throw new ArgumentException("Hay productos no válidos en la orden.");
            }

            Orden modeloOrden = nuevaOrden.ComoNuevoModelo(cliente);

            _contexto.Add(modeloOrden);
            await _contexto.SaveChangesAsync();

            List<ProductosOrdenados> productosOrdenados = new List<ProductosOrdenados>();

            foreach(var producto in productosExistentes)
            {
                int cantidad = nuevaOrden.Productos.ToList()
                    .Find(p => p.IdProducto == producto.Id)?.Cantidad ?? 0;

                var productoOrdenado = new ProductosOrdenados
                {
                    Cantidad = cantidad,
                    IdProducto = producto.Id,
                    Producto = producto,
                    IdOrden = modeloOrden.Id,
                    Orden = modeloOrden,
                };

                productosOrdenados.Add(productoOrdenado);
            }

            _contexto.AddRange(productosOrdenados);
            await _contexto.SaveChangesAsync();

            return modeloOrden.ComoDTO();
        }
    }
}
#nullable disable
