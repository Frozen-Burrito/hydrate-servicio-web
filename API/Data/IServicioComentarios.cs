using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

#nullable enable
namespace ServicioHydrate.Data
{
    public interface IServicioComentarios 
    {
        Task<ICollection<DTOComentario>> GetComentarios(Guid? idUsuarioActual, DTOParamsPagina? paramsPagina, bool soloPublicados = true);

        Task<DTOComentario> GetComentarioPorId(int idComentario, Guid? idUsuarioActual);

        Task<ICollection<DTOComentario>> GetComentariosPorUsuario(Guid idUsuario, Guid? idUsuarioActual, DTOParamsPagina? paramsPagina);

        Task<ICollection<DTOComentario>> GetComentariosPendientes(DTOParamsPagina? paramsPagina);

        Task<DTOComentario> AgregarNuevoComentario(DTONuevoComentario comentario, Guid? idAutor, bool autoPublicar = false);

        Task<DTOComentarioArchivado> ArchivarComentario(int idComentario, DTOArchivarComentario motivos);

        Task<ICollection<DTOComentarioArchivado>> GetMotivosDeComentariosArchivados(Guid idUsuario, DTOParamsPagina? paramsPagina);

        Task PublicarComentarioPendiente(int idComentario);

        Task<DTOComentario> ActualizarComentario(DTOComentario comentarioModificado, Guid? idUsuarioActual);

        Task EliminarComentario(int idComentario, Guid? idUsuario, string rolDeUsuario);

        Task MarcarComentarioComoUtil(int idComentario, Guid idUsuarioActual);

        Task ReportarComentario(int idComentario, Guid idUsuarioActual);

        Task<ICollection<DTORespuesta>> GetRespuestasDeComentario(int idComentario, Guid? idUsuarioActual, DTOParamsPagina? paramsPagina);

        Task<DTORespuesta> GetRespuestaPorId(int idComentario, int idRespuesta, Guid? idUsuarioActual);

        Task<DTORespuesta> AgregarNuevaRespuesta(int idComentario, DTONuevaRespuesta respuesta, Guid idAutor, bool autoPublicar = false);

        Task EliminarRespuesta(int idComentario, int idRespuesta, Guid idUsuario, string rolUsuario);

        Task MarcarRespuestaComoUtil(int idComentario, int idRespuesta, Guid idUsuarioActual);

        Task ReportarRespuesta(int idComentario, int idRespuesta, Guid idUsuarioActual);
    }
}
#nullable disable