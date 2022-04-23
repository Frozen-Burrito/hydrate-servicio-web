using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Data
{
    public class RepositorioComentarios : IServicioComentarios
    {
        private readonly ContextoDBSqlite _contexto;

        public RepositorioComentarios(ContextoDBSqlite contexto)
        {
            this._contexto = contexto;
        }

        public async Task<DTOComentario> ActualizarComentario(DTOComentario comentarioModificado, Guid? idUsuarioActual)
        {
            var comentario = await _contexto.Comentarios
                .Where(c => c.Id == comentarioModificado.Id)
                .Include(c => c.Autor)
                .Include(c => c.ReportesDeUsuarios)
                .Include(c => c.UtilParaUsuarios)
                .AsSplitQuery()
                .FirstAsync();

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            comentario.Actualizar(comentarioModificado);

            _contexto.Entry(comentario).State = EntityState.Modified;

            await _contexto.SaveChangesAsync();

            return comentario.ComoDTO(idUsuarioActual);
        }

        public async Task<DTORespuesta> AgregarNuevaRespuesta(int idComentario, DTONuevaRespuesta respuesta, Guid idAutor)
        {
            Comentario comentario = await _contexto.Comentarios
                .Where(c => c.Publicado && c.Id == idComentario)
                .Include(c => c.Respuestas)
                .FirstAsync();

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            Usuario autorRespuesta = await _contexto.Usuarios.FindAsync(idAutor);

            if (autorRespuesta is null)
            {
                throw new ArgumentException("No existe un usuario con el ID recibido");
            }

            Respuesta modeloRespuesta = respuesta.ComoNuevoModelo(comentario, autorRespuesta);

            comentario.Respuestas.Add(modeloRespuesta);
            await _contexto.SaveChangesAsync();

            return modeloRespuesta.ComoDTO(null);
        }

        public async Task<DTOComentario> AgregarNuevoComentario(DTONuevoComentario comentario, Guid? idAutor)
        {
            var autorComentario = await _contexto.Usuarios.FindAsync(idAutor);

            if (autorComentario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID recibido");
            }

            var modeloComentario = comentario.ComoNuevoModelo(autorComentario);

            _contexto.Add(modeloComentario);
            await _contexto.SaveChangesAsync();

            return modeloComentario.ComoDTO(idAutor);
        }

        public async Task EliminarComentario(int idComentario, Guid? idUsuario, string rolDeUsuario)
        {
            var comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            comentario = await _contexto.Comentarios
                .Where(c => c.Id == idComentario)
                .Include(c => c.Autor)
                .FirstAsync();

            if (!idUsuario.Equals(comentario.Autor.Id) && !rolDeUsuario.Equals(RolDeUsuario.MODERADOR_COMENTARIOS.ToString()))
            {
                // El usuario intentando eliminar el comentario no es el autor ni un moderador.
                throw new InvalidOperationException("No es posible eliminar el comentario con las credenciales utilizadas.");
            }

            _contexto.Remove(comentario);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarRespuesta(int idComentario, int idRespuesta, Guid idUsuario, string rolDeUsuario)
        {
            var comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            var respuesta = await _contexto.Respuestas.FindAsync(idRespuesta);

            if (respuesta is null)
            {
                throw new ArgumentException("No existe una respuesta con el ID especificado.");
            }

            respuesta = await _contexto.Respuestas
                .Where(r => r.Id == idRespuesta)
                .Include(r => r.Autor)
                .FirstAsync();

            bool rolesCoinciden = respuesta.Autor.RolDeUsuario.ToString().Equals(rolDeUsuario);
            bool usuarioEsModerador = rolDeUsuario.Equals(RolDeUsuario.MODERADOR_COMENTARIOS.ToString());

            if (!rolesCoinciden || (!idUsuario.Equals(respuesta.Autor.Id) && !usuarioEsModerador))
            {
                throw new InvalidOperationException("Solo el mismo autor o un moderador puede borrar una respuesta.");
            }

            _contexto.Remove(respuesta);
            await _contexto.SaveChangesAsync();
        }

        public async Task<DTOComentario> GetComentarioPorId(int idComentario, Guid? idUsuarioActual)
        {
            var comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            comentario = await _contexto.Comentarios
                .Where(c => c.Id == idComentario)
                .Include(c => c.Autor)
                .Include(c => c.UtilParaUsuarios)
                .Include(c => c.ReportesDeUsuarios)
                .FirstAsync();

            return comentario.ComoDTO(idUsuarioActual);
        }

        public async Task<List<DTOComentario>> GetComentarios(Guid? idUsuarioActual, bool publicados = true)
        {
            if (_contexto.Comentarios.Count() <= 0)
            {
                // Si no existe ningun comentario, retornar una lista vacia desde el principio.
                return new List<DTOComentario>();
            }

            var comentarios = _contexto.Comentarios
                .Where(c => c.Publicado == publicados)
                .OrderByDescending(c => c.Fecha)
                .Include(c => c.Autor)
                .Include(c => c.UtilParaUsuarios)
                .Include(c => c.ReportesDeUsuarios)
                .AsSplitQuery()
                .Select(c => c.ComoDTO(idUsuarioActual));

            return await comentarios.ToListAsync();
        }

        public async Task<List<DTOComentario>> GetComentariosPorUsuario(Guid idUsuario, Guid? idUsuarioActual)
        {
            if (_contexto.Comentarios.Count() <= 0)
            {
                // Si no existe ningun comentario, retornar una lista vacia desde el principio.
                return new List<DTOComentario>();
            }

            var usuarioAutor = await _contexto.Usuarios.FindAsync(idUsuario);

            if (usuarioAutor is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            var comentariosDelAutor = _contexto.Comentarios
                .Where(c => c.Autor == usuarioAutor)
                .OrderByDescending(c => c.Fecha)
                .Include(c => c.Autor)
                .Include(c => c.UtilParaUsuarios)
                .Include(c => c.ReportesDeUsuarios)
                .AsSplitQuery()
                .Select(c => c.ComoDTO(idUsuarioActual));

            return await comentariosDelAutor.ToListAsync();
        }

        public async Task<DTORespuesta> GetRespuestaPorId(int idComentario, int idRespuesta, Guid? idUsuarioActual)
        {
            Respuesta respuesta = await _contexto.Respuestas.FindAsync(idRespuesta);

            if (respuesta is null)
            {
                throw new ArgumentException("No existe una respuesta con el ID especificado en el comentario.");
            }

            respuesta = await _contexto.Respuestas
                .Where(r => (r.IdComentario == idComentario && r.Id == idRespuesta))
                .Include(r => r.Autor)
                .Include(r => r.ReportesDeUsuarios)
                .Include(r => r.UtilParaUsuarios)
                .FirstAsync();

            return respuesta.ComoDTO(idUsuarioActual);
        }

        public async Task<List<DTORespuesta>> GetRespuestasDeComentario(int idComentario, Guid? idUsuarioActual)
        {
            int numRespuestas = await _contexto.Respuestas.CountAsync();

            if (numRespuestas <= 0)
            {
                // Si no existe ninguna respuesta, retornar una lista vacia desde el principio.
                return new List<DTORespuesta>();
            }

            Comentario comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            List<DTORespuesta> dtosRespuestas = await _contexto.Respuestas
                .Where(r => r.IdComentario == idComentario)
                .Include(r => r.Autor)
                .Include(r => r.UtilParaUsuarios)
                .Include(r => r.ReportesDeUsuarios)
                .AsSplitQuery()
                .OrderBy(r => r.Fecha)
                .Select(r => r.ComoDTO(idUsuarioActual))
                .ToListAsync();

            return dtosRespuestas;
        }

        public async Task MarcarComentarioComoUtil(int idComentario, Guid idUsuarioActual)
        {
            Comentario comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            comentario = await _contexto.Comentarios
                .Where(c => c.Id == idComentario)
                .Include(c => c.UtilParaUsuarios)
                .FirstAsync();

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            Usuario usuario = await _contexto.Usuarios
                .Where(u => u.Id == idUsuarioActual)
                .Include(u => u.ComentariosUtiles)
                .FirstAsync();
            
            if (usuario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            bool existenMarcadores = comentario.UtilParaUsuarios.Count > 0;
            Usuario usuarioEnUtiles = null;

            if (existenMarcadores)
            {
                usuarioEnUtiles = comentario.UtilParaUsuarios
                    .Where(u => u.Id == usuario.Id)
                    .First();
            }

            if (!existenMarcadores || usuarioEnUtiles is null)
            {
                // El usuario aún no ha marcado como útil el comentario.
                comentario.UtilParaUsuarios.Add(usuario);
                usuario.ComentariosUtiles.Add(comentario);
            }
            else 
            {
                // El usuario tiene marcado como útil este comentario.
                comentario.UtilParaUsuarios.Remove(usuario);
                usuario.ComentariosUtiles.Remove(comentario);
            }

            // Marcar la entidad del comentario como modificada.
            _contexto.Entry(comentario).State = EntityState.Modified;

            // Guardar cambios en entidades del contexto.
            await _contexto.SaveChangesAsync();
        }

        public async Task MarcarRespuestaComoUtil(int idComentario, int idRespuesta, Guid idUsuarioActual)
        {
            var comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            Respuesta respuesta = await _contexto.Respuestas.FindAsync(idRespuesta);

            if (respuesta is null)
            {
                throw new ArgumentException("No existe una respuesta con el ID especificado.");
            }

            respuesta = await _contexto.Respuestas
                .Where(r => r.Id == idRespuesta)
                .Include(r => r.UtilParaUsuarios)
                .FirstAsync();            

            var usuario = await _contexto.Usuarios
                .Where(u => u.Id == idUsuarioActual)
                .Include(u => u.RespuestasUtiles)
                .FirstAsync();
            
            if (usuario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            bool existenMarcadores = respuesta.UtilParaUsuarios.Count > 0;
            Usuario usuarioEnUtiles = null;

            if (existenMarcadores)
            {
                usuarioEnUtiles = respuesta.UtilParaUsuarios
                    .Where(u => u.Id == usuario.Id)
                    .First();
            }

            if (!existenMarcadores || usuarioEnUtiles is null)
            {
                // El usuario aún no ha marcado como útil esta respuesta.
                respuesta.UtilParaUsuarios.Add(usuario);
                usuario.RespuestasUtiles.Add(respuesta);
            }
            else 
            {
                // El usuario tiene marcado como útil esta respuesta.
                respuesta.UtilParaUsuarios.Remove(usuario);
                usuario.RespuestasUtiles.Remove(respuesta);
            }

            // Marcar la entidad de la respuesta como modificada.
            _contexto.Entry(respuesta).State = EntityState.Modified;

            // Guardar cambios en entidades del contexto.
            await _contexto.SaveChangesAsync();
        }

        public async Task ReportarComentario(int idComentario, Guid idUsuarioActual)
        {
            Comentario comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            comentario = await _contexto.Comentarios
                .Where(c => c.Id == idComentario)
                .Include(c => c.ReportesDeUsuarios)
                .FirstAsync();

            Usuario usuario = await _contexto.Usuarios
                .Where(u => u.Id == idUsuarioActual)
                .Include(u => u.ComentariosReportados)
                .FirstAsync();
            
            if (usuario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            bool existenReportes = comentario.ReportesDeUsuarios.Count > 0;
            Usuario usuarioEnReportes = null;

            if (existenReportes)
            {
                // Obtener la entidad del usuario actual desde la lista de reportes del comentario.
                usuarioEnReportes = comentario.ReportesDeUsuarios
                    .Where(u => u.Id == usuario.Id)
                    .First();
            }
            

            // Revisar si existe el usuario en los reportes del comentario.
            if (!existenReportes || usuarioEnReportes is null)
            {
                // El usuario aún no ha reportado este comentario.
                comentario.ReportesDeUsuarios.Add(usuario);
                usuario.ComentariosReportados.Add(comentario);
            }
            else 
            {
                // El usuario ya ha reportado este comentario.
                comentario.ReportesDeUsuarios.Remove(usuario);
                usuario.ComentariosReportados.Remove(comentario);
            }

            // Marcar la entidad del comentario como modificada.
            _contexto.Entry(comentario).State = EntityState.Modified;

            // Guardar cambios en entidades del contexto.
            await _contexto.SaveChangesAsync();
        }

        public async Task ReportarRespuesta(int idComentario, int idRespuesta, Guid idUsuarioActual)
        {
            var comentario = await _contexto.Comentarios.FindAsync(idComentario);

            if (comentario is null)
            {
                throw new ArgumentException("No existe un comentario con el ID especificado.");
            }

            Respuesta respuesta = await _contexto.Respuestas.FindAsync(idRespuesta);

            if (respuesta is null)
            {
                throw new ArgumentException("No existe una respuesta con el ID especificado.");
            }

            respuesta = await _contexto.Respuestas
                .Where(r => r.Id == idRespuesta)
                .Include(r => r.ReportesDeUsuarios)
                .FirstAsync();

            var usuario = await _contexto.Usuarios
                .Where(u => u.Id == idUsuarioActual)
                .Include(u => u.RespuestasReportadas)
                .FirstAsync();
            
            if (usuario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            bool existenReportes = respuesta.ReportesDeUsuarios.Count > 0;
            Usuario usuarioEnReportes = null;

            if (existenReportes)
            {
                // Obtener la entidad del usuario actual desde la lista de reportes del respuesta.
                usuarioEnReportes = respuesta.ReportesDeUsuarios
                    .Where(u => u.Id == usuario.Id)
                    .First();
            }

            // Revisar si existe el usuario en los reportes de esta respuesta.
            if (!existenReportes || usuarioEnReportes is null)
            {
                // El usuario aún no ha reportado esta respuesta.
                respuesta.ReportesDeUsuarios.Add(usuario);
                usuario.RespuestasReportadas.Add(respuesta);
            }
            else 
            {
                // El usuario ya ha reportado esta respuesta.
                respuesta.ReportesDeUsuarios.Remove(usuario);
                usuario.RespuestasReportadas.Remove(respuesta);
            }

            // Marcar la entidad de la respuesta como modificada.
            _contexto.Entry(respuesta).State = EntityState.Modified;

            // Guardar cambios en entidades del contexto.
            await _contexto.SaveChangesAsync();
        }
    }
}