using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data
{
    public interface IServicioComentarios 
    {
        Task<List<DTOComentario>> GetComentarios(Guid? idUsuarioActual, bool publicados = true);

        Task<DTOComentario> GetComentarioPorId(int idComentario, Guid? idUsuarioActual);

        Task<List<DTOComentario>> GetComentariosPorUsuario(Guid idUsuario, Guid? idUsuarioActual);

        Task<List<DTOComentario>> GetComentariosPendientes();

        Task<DTOComentario> AgregarNuevoComentario(DTONuevoComentario comentario, Guid? idAutor);

        Task<DTOComentario> ActualizarComentario(DTOComentario comentarioModificado, Guid? idUsuarioActual);

        Task EliminarComentario(int idComentario, Guid? idUsuario, string rolDeUsuario);

        Task MarcarComentarioComoUtil(int idComentario, Guid idUsuarioActual);

        Task ReportarComentario(int idComentario, Guid idUsuarioActual);

        Task<List<DTORespuesta>> GetRespuestasDeComentario(int idComentario, Guid? idUsuarioActual);

        Task<DTORespuesta> GetRespuestaPorId(int idComentario, int idRespuesta, Guid? idUsuarioActual);

        Task<DTORespuesta> AgregarNuevaRespuesta(int idComentario, DTONuevaRespuesta respuesta, Guid idAutor);

        Task EliminarRespuesta(int idComentario, int idRespuesta, Guid idUsuario, string rolUsuario);

        Task MarcarRespuestaComoUtil(int idComentario, int idRespuesta, Guid idUsuarioActual);

        Task ReportarRespuesta(int idComentario, int idRespuesta, Guid idUsuarioActual);
    }
}