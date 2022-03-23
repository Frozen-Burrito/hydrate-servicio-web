using System;
using System.Globalization;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    /// Esta clase proporciona métodos de utilidad para facilitar la 
    /// conversión entre objetos de modelo y DTOs.
    public static class Extensiones
    {
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
                // FechaPublicacion = DateTime.Parse(dtoRecurso.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None),
                FechaPublicacion = DateTime.Parse(dtoRecurso.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("O"),
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
                FechaPublicacion = DateTime.Parse(recurso.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("O"),
            };
        }

        public static void Actualizar(this RecursoInformativo recurso, DTORecursoInformativo modificaciones)
        {
            recurso.Titulo = modificaciones.Titulo;
            recurso.Url = modificaciones.Url;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.Descripcion = modificaciones.Descripcion;
            recurso.FechaPublicacion = DateTime.Parse(modificaciones.FechaPublicacion, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("O");
        }
    }
}