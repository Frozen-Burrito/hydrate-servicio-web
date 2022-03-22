using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data 
{
    public class RepositorioRecursos : IServicioRecursos
    {
        private readonly ContextoDB _contexto;

        public RepositorioRecursos(ContextoDB contexto)
        {
            this._contexto = contexto;
        }

        public async Task<DTORecursoInformativo> ActualizarRecursoAsync(int idRecurso, DTORecursoInformativo recursoActualizado)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DTORecursoInformativo> AgregarNuevoRecursoAsync(DTORecursoInformativo nuevoRecurso)
        {
            throw new System.NotImplementedException();
        }

        public async Task EliminarRecursoAsync(int idRecurso)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DTORecursoInformativo> GetRecursoPorIdAsync(int idRecurso)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<DTORecursoInformativo>> GetRecursosAsync()
        {
            var recursos = _contexto.Recursos.Select(r => r.ComoDTO());

            return recursos.ToListAsync();
        }
    }
}