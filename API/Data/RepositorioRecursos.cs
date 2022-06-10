using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data 
{
    public class RepositorioRecursos : IServicioRecursos
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioRecursos(ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public async Task ActualizarRecursoAsync(DTORecursoInformativo recursoModificado)
        {
            RecursoInformativo? modeloRecurso = await _contexto.Recursos.FindAsync(recursoModificado.Id);

            if (modeloRecurso is null)
            {
                throw new ArgumentException("No existe un recurso informativo con el ID especificado.");
            }

            modeloRecurso.Actualizar(recursoModificado);

            _contexto.Entry(modeloRecurso).State = EntityState.Modified;

            await _contexto.SaveChangesAsync();
        }

        public async Task<DTORecursoInformativo> AgregarNuevoRecursoAsync(DTORecursoInformativo nuevoRecurso)
        {
            RecursoInformativo modeloRecurso = nuevoRecurso.ComoModelo();
            
            _contexto.Add(modeloRecurso);
            await _contexto.SaveChangesAsync();

            return modeloRecurso.ComoDTO();
        }

        public async Task EliminarRecursoAsync(int idRecurso)
        {
            var recurso = await _contexto.Recursos.FindAsync(idRecurso);

            if (recurso is null)
            {
                throw new ArgumentException("No existe un recurso informativo con el ID especificado.");
            }

            _contexto.Remove(recurso);
            await _contexto.SaveChangesAsync();
        }

        public async Task<DTORecursoInformativo> GetRecursoPorIdAsync(int idRecurso)
        {
            var recurso = await _contexto.Recursos.FindAsync(idRecurso);

            if (recurso is null)
            {
                throw new ArgumentException("No existe un recurso informativo con el ID especificado.");
            }

            return recurso.ComoDTO();
        }

        public async Task<ICollection<DTORecursoInformativo>> GetRecursosAsync(DTOParamsPagina? paramsPagina)
        {
            var recursos = _contexto.Recursos
                .OrderByDescending(r => r.FechaPublicacion)
                .Select(r => r.ComoDTO());

            var recursosPaginados = await ListaPaginada<DTORecursoInformativo>
                .CrearAsync(recursos, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return recursosPaginados;
        }
    }
}
#nullable disable