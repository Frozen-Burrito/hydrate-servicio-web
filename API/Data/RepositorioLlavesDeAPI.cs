
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data 
{
	public class RepositorioLlavesDeAPI : IServicioLlavesDeAPI
	{
		// El contexto de EF para la base de datos.
        private readonly ContextoDBSqlite _contexto;

        public RepositorioLlavesDeAPI(ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public async Task<LlaveDeApi?> BuscarLlave(string llaveProporcionada)
        {
			if (await _contexto.LlavesDeAPI.CountAsync() <= 0) 
            {
                // No hay ninguna llave registrada.
                return null;
            }

            LlaveDeApi? llave = await _contexto.LlavesDeAPI
                .Where(ll => ll.Llave.Equals(llaveProporcionada))
                .Include(ll => ll.Usuario)
                .FirstOrDefaultAsync();

            return llave;
        }

        public async Task EliminarLlave(Guid idUsuario, string llave)
        {
            LlaveDeApi? llaveEncontrada = await BuscarLlave(llave);

            if (llaveEncontrada is not null)
            {
                _contexto.LlavesDeAPI.Remove(llaveEncontrada);
                await _contexto.SaveChangesAsync();
            }
        }

        public async Task GenerarNuevaLlave(Guid idUsuario)
        {
            var llavesDelUsuario = await GetLlavesDeUsuario(idUsuario);

            if (llavesDelUsuario.Count >= 3)
            {
                throw new InvalidOperationException("El usuario ya tiene registradas 3 llaves de API.");
            }

            LlaveDeApi nuevaLlave = new LlaveDeApi(
                0, 
                idUsuario,
                Guid.NewGuid().ToString(),
                DateTime.Now
            );

            _contexto.LlavesDeAPI.Add(nuevaLlave);
            await _contexto.SaveChangesAsync();
        }

        public async Task<ICollection<DTOLlaveDeAPI>> GetLlavesDeUsuario(Guid idUsuario)
        {
            if (await _contexto.LlavesDeAPI.CountAsync() <= 0) 
            {
                // No hay ninguna llave registrada.
                return new List<DTOLlaveDeAPI>();
            }

            var llavesDeApiDeUsuario = await _contexto.LlavesDeAPI
                .Where(ll => ll.IdUsuario.Equals(idUsuario))
                .Select(ll => ll.ComoDTO())
                .ToListAsync();

            return llavesDeApiDeUsuario;
        }

        public async Task<ICollection<DTOLlaveDeAPI>> GetTodasLasLlaves(DTOParamsPagina paramsPagina)
        {
            if (await _contexto.LlavesDeAPI.CountAsync() <= 0) 
            {
                // No hay ninguna llave registrada.
                return new ListaPaginada<DTOLlaveDeAPI>();
            }

            IQueryable<DTOLlaveDeAPI> todasLasLlaves = _contexto.LlavesDeAPI
                .Select(ll => ll.ComoDTO());

            var llavesPaginadas = await ListaPaginada<DTOLlaveDeAPI>
                .CrearAsync(todasLasLlaves, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return llavesPaginadas;
        }
    }
}  
#nullable disable