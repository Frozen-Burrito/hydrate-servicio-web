using System;
using System.Linq;
using System.Globalization;
using ServicioHydrate.Modelos.DTO;
using System.Collections.Generic;

namespace ServicioHydrate.Modelos
{
    /// Esta clase proporciona métodos de utilidad para facilitar la 
    /// conversión entre objetos de modelo y DTOs.
    public static class Extensiones
    {
        public static string VerificarStrISO8601(string strFecha)
        {
            DateTime fecha;

            bool strISO8601Valido = DateTime.TryParse(strFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (strISO8601Valido)
            {
                return fecha.ToString("O");
            }
            else 
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }
        }

        public static Usuario ComoModelo(this DTOPeticionAutenticacion usuario, string hashContrasenia)
        {
            // Lanzar una excepción de argumento si la contraseña y el hash son iguales.
            if (usuario.Password.Equals(hashContrasenia))
            {
                throw new ArgumentException("La contraseña y el hash de la contraseña no deben ser iguales.");
            }

            return new Usuario 
            {
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Password = hashContrasenia,
            };
        }

        public static DTOUsuario ComoDTO(this Usuario usuario)
        {
            return new DTOUsuario 
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                RolDeUsuario = usuario.RolDeUsuario
            };
        }

        public static Usuario ComoModelo(this DTOUsuario dtoUsuario)
        {
            return new Usuario
            {
                Id = dtoUsuario.Id,
                Email = dtoUsuario.Email,
                NombreUsuario = dtoUsuario.NombreUsuario,
            };
        }

        public static RecursoInformativo ComoModelo(this DTORecursoInformativo dtoRecurso)
        {
            return new RecursoInformativo
            {
                Id = dtoRecurso.Id,
                Titulo = dtoRecurso.Titulo,
                Url = dtoRecurso.Url,
                Descripcion = dtoRecurso.Descripcion,
                FechaPublicacion = VerificarStrISO8601(dtoRecurso.FechaPublicacion),
            };
        }

        public static DTORecursoInformativo ComoDTO(this RecursoInformativo recurso)
        {
            return new DTORecursoInformativo
            {
                Id = recurso.Id,
                Titulo = recurso.Titulo,
                Url = recurso.Url,
                Descripcion = recurso.Descripcion,
                FechaPublicacion = VerificarStrISO8601(recurso.FechaPublicacion),
            };
        }

        public static void Actualizar(this RecursoInformativo recurso, DTORecursoInformativo modificaciones)
        {
            recurso.Titulo = modificaciones.Titulo;
            recurso.Url = modificaciones.Url;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.FechaPublicacion = VerificarStrISO8601(modificaciones.FechaPublicacion);
        }

        public static Comentario ComoNuevoModelo(this DTONuevoComentario nuevoComentario, Usuario autor)
        {
            //TODO: Verificar el contenido del comentario
            // Si el comentario no es apto, crear el comentario y marcar "Publicado" como false.
            // Si el contenido es apto, crear el comentario y marcar "Publicado" como true.
            bool contenidoAdecuado = true;

            return new Comentario
            {
                Asunto = nuevoComentario.Asunto,
                Contenido = nuevoComentario.Contenido,
                Autor = autor,
                Fecha = DateTime.Now.ToString("o"),
                Publicado = contenidoAdecuado,
                UtilParaUsuarios = new List<Usuario>(),
                ReportesDeUsuarios = new List<Usuario>(),
                Respuestas = new List<Respuesta>(),
            };
        }

        public static DTOComentario ComoDTO(this Comentario comentario, Guid? idUsuarioActual)
        {
            bool reportadoPorUsuarioActual = false;
            bool utilParaUsuarioActual = false;

            // Verificar directamente si el usuario ha marcado como util o reportado 
            // el comentario, para evitar pasar al cliente toda la lista de usuarios que han
            // hecho algo con el comentario.
            if (idUsuarioActual is not null)
            {
                if (comentario.ReportesDeUsuarios.Count > 0)
                {
                    var usuarioEnReportes = comentario.ReportesDeUsuarios.Where(u => u.Id == idUsuarioActual).FirstOrDefault();
                    
                    // Si existe un usuario en los ReportesDeUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido reportado por el usuario.
                    reportadoPorUsuarioActual = (usuarioEnReportes is not null);
                }

                if (comentario.UtilParaUsuarios.Count > 0)
                {
                    var usuarioEnUtil = comentario.UtilParaUsuarios.Where(u => u.Id == idUsuarioActual).FirstOrDefault();

                    // Si existe un usuario en UtilParaUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido marcado como util por el usuario.
                    utilParaUsuarioActual = (usuarioEnUtil is not null);
                }
            }

            return new DTOComentario
            {
                Id = comentario.Id,
                Asunto = comentario.Asunto,
                Contenido = comentario.Contenido,
                IdAutor = comentario.Autor.Id,
                Fecha = VerificarStrISO8601(comentario.Fecha),
                Publicado = comentario.Publicado,
                NumeroDeReportes = comentario.ReportesDeUsuarios.Count,
                ReportadoPorUsuarioActual = reportadoPorUsuarioActual,
                NumeroDeUtil = comentario.UtilParaUsuarios.Count,
                UtilParaUsuarioActual = utilParaUsuarioActual,
            };
        }

        public static void Actualizar(this Comentario modelo, DTOComentario modificaciones)
        {
            modelo.Asunto = modificaciones.Asunto;
            modelo.Contenido = modificaciones.Contenido;
            modelo.Publicado = modificaciones.Publicado;
            modelo.Fecha = VerificarStrISO8601(modificaciones.Fecha);
        }

        public static Respuesta ComoNuevoModelo(this DTONuevaRespuesta nuevaRespuesta, Comentario comentarioOriginal, Usuario autor)
        {
            bool contenidoAdecuado = true;

            return new Respuesta
            {
                Contenido = nuevaRespuesta.Contenido,
                Autor = autor,
                Fecha = DateTime.Now.ToString("o"),
                Publicado = contenidoAdecuado,
                UtilParaUsuarios = new List<Usuario>(),
                ReportesDeUsuarios = new List<Usuario>(),
                Comentario = comentarioOriginal,
                IdComentario = comentarioOriginal.Id,
            };
        }

        public static DTORespuesta ComoDTO(this Respuesta respuesta, Guid? idUsuarioActual)
        {
            bool reportadoPorUsuarioActual = false;
            bool utilParaUsuarioActual = false;

            // Verificar directamente si el usuario ha marcado como util o reportado 
            // el comentario, para evitar pasar al cliente toda la lista de usuarios que han
            // hecho algo con el comentario.
            if (idUsuarioActual is not null)
            {
                if (respuesta.ReportesDeUsuarios.Count > 0)
                {
                    var usuarioEnReportes = respuesta.ReportesDeUsuarios.Where(u => u.Id == idUsuarioActual).First();
                    
                    // Si existe un usuario en los ReportesDeUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido reportado por el usuario.
                    reportadoPorUsuarioActual = (usuarioEnReportes is not null);
                }

                if (respuesta.UtilParaUsuarios.Count > 0)
                {
                    var usuarioEnUtil = respuesta.UtilParaUsuarios.Where(u => u.Id == idUsuarioActual).First();

                    // Si existe un usuario en UtilParaUsuarios del comentario con el
                    // Id del usuario actual, el comentario ha sido marcado como util por el usuario.
                    utilParaUsuarioActual = (usuarioEnUtil is not null);
                }
            }

            return new DTORespuesta
            {
                Id = respuesta.Id,
                Contenido = respuesta.Contenido,
                Fecha = respuesta.Fecha,
                Publicada = respuesta.Publicado,
                IdAutor = respuesta.Autor.Id,
                NumeroDeReportes = respuesta.ReportesDeUsuarios.Count,
                NumeroDeUtil = respuesta.UtilParaUsuarios.Count,
                ReportadaPorUsuarioActual = reportadoPorUsuarioActual,
                UtilParaUsuarioActual = utilParaUsuarioActual
            };
        }
    }
}