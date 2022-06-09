using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data 
{
    public interface IServicioRecursos
    {
        Task<ICollection<DTORecursoInformativo>> GetRecursosAsync(DTOParamsPagina? paramsPagina);
        Task<DTORecursoInformativo> GetRecursoPorIdAsync(int idRecurso);
        Task<DTORecursoInformativo> AgregarNuevoRecursoAsync(DTORecursoInformativo nuevoRecurso);
        Task ActualizarRecursoAsync(DTORecursoInformativo recursoActualizado);
        Task EliminarRecursoAsync(int idRecurso);
    }
}
#nullable disable