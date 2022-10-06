
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
        private readonly ContextoDBMysql _contexto;

        public RepositorioLlavesDeAPI(ContextoDBMysql contexto)
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

        public async Task EliminarLlave(Guid idUsuario, int idLlave)
        {
            LlaveDeApi? llave = await _contexto.LlavesDeAPI.FindAsync(idLlave);

            if (llave is null)
            {
                throw new ArgumentNullException("No existe una llave con el ID especificado.");
            }

            _contexto.LlavesDeAPI.Remove(llave);
            await _contexto.SaveChangesAsync();
        }

        public async Task GenerarNuevaLlave(Guid idUsuario, string nombreLlave)
        {
            var llavesDelUsuario = await GetLlavesDeUsuario(idUsuario);

            if (llavesDelUsuario.Count >= 3)
            {
                throw new InvalidOperationException("El usuario ya tiene registradas 3 llaves de API.");
            }

            LlaveDeApi nuevaLlave = new LlaveDeApi(
                0, 
                idUsuario,
                nombreLlave,
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

        public async Task<ICollection<DTOLlaveDeAPIAdmin>> GetTodasLasLlaves(DTOParamsPagina paramsPagina)
        {
            if (await _contexto.LlavesDeAPI.CountAsync() <= 0) 
            {
                // No hay ninguna llave registrada.
                return new ListaPaginada<DTOLlaveDeAPIAdmin>();
            }

            IQueryable<DTOLlaveDeAPIAdmin> todasLasLlaves = _contexto.LlavesDeAPI
                .Include(ll => ll.Usuario)
                .Select(ll => ll.ComoDTOAdmin());

            var llavesPaginadas = await ListaPaginada<DTOLlaveDeAPIAdmin>
                .CrearAsync(todasLasLlaves, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return llavesPaginadas;
        }

        public async Task<DTOStatsLlavesDeApi> GetUsoDeAPI()
        {
            if (await _contexto.LlavesDeAPI.CountAsync() <= 0) 
            {
                // No hay ninguna llave registrada.
                return new DTOStatsLlavesDeApi();
            }

            IQueryable<LlaveDeApi> llaves = _contexto.LlavesDeAPI.AsQueryable();

            int peticionesTotales = await llaves
                .SumAsync(ll => ll.PeticionesEnMes);

            int erroresTotales = await llaves
                .SumAsync(ll => ll.ErroresEnMes);

            int clientesActivos = await llaves
                .Where(ll => ll.PeticionesEnMes > 0)
                .CountAsync();

            return new DTOStatsLlavesDeApi
            {
                Plazo = "mes",
                PeticionesTotales = peticionesTotales,
                ErroresTotales = erroresTotales,
                NumDeLlavesActivas = clientesActivos,
            };
        }

        public async Task<DTOLlaveDeAPI> RegenerarLlave(int idLlave)
        {
            LlaveDeApi? llaveDeApi = await _contexto.LlavesDeAPI.FindAsync(idLlave);

            if (llaveDeApi is null)
            {
                throw new InvalidOperationException("La llave de API no fue encontrada.");
            }

            llaveDeApi.RegenerarLlave();

            _contexto.Entry(llaveDeApi).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();

            return llaveDeApi.ComoDTO();
        }

        public async Task RegistrarError(string llave)
        {
            LlaveDeApi? llaveDeApi = await BuscarLlave(llave);

            if (llaveDeApi is null)
            {
                throw new InvalidOperationException("La llave de API no fue encontrada.");
            }

            llaveDeApi.RegistrarError();

            _contexto.Entry(llaveDeApi).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }

        public async Task RegistrarPeticion(string llave)
        {
            LlaveDeApi? llaveDeApi = await BuscarLlave(llave);

            if (llaveDeApi is null)
            {
                throw new InvalidOperationException("La llave de API no fue encontrada.");
            }

            llaveDeApi.RegistrarUso();

            _contexto.Entry(llaveDeApi).State = EntityState.Modified;
            await _contexto.SaveChangesAsync();
        }
    }
}  
#nullable disable