using System;
using System.Globalization;
using System.Linq;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public static class Extensiones
    {
        public static Usuario ComoModelo(this DTOUsuario usuario, string hashContrasenia)
        {
            return new Usuario 
            {
                Id = usuario.Id,
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
                Email = usuario.Email
            };
        }

        public static DTORespuestaAutenticacion ComoRespuestaToken(this Usuario usuario)
        {
            return new DTORespuestaAutenticacion
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Token = "Token default"
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
                FechaPublicacion = DateTime.Parse(dtoRecurso.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None),
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
                FechaPublicacion = recurso.FechaPublicacion.ToString("O"),
            };
        }

        public static void Actualizar(this RecursoInformativo recurso, DTORecursoInformativo modificaciones)
        {
            recurso.Titulo = modificaciones.Titulo;
            recurso.Url = modificaciones.Url;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.FechaPublicacion = DateTime.Parse(modificaciones.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}