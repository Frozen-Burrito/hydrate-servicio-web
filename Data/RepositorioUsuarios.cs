using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Autenticacion;

namespace ServicioHydrate.Data
{
    public class RepositorioUsuarios : IServicioUsuarios
    {
        private readonly ContextoDB _contexto;
        private readonly GeneradorDeToken _generadorToken;

        public RepositorioUsuarios(ContextoDB contexto, GeneradorDeToken generadorToken)
        {
            this._contexto = contexto;
            this._generadorToken = generadorToken;
        }

        public Task<DTOUsuario> ActualizarUsuarioAsync(DTOUsuario dtoUsuario)
        {
            throw new NotImplementedException();
        }

        public async Task<DTORespuestaAutenticacion> AutenticarUsuario(DTOPeticionAutenticacion infoUsuario)
        {
            if (string.IsNullOrEmpty(infoUsuario.Email) && string.IsNullOrEmpty(infoUsuario.NombreUsuario))
            {
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.FORMATO_INCORRECTO,
                    Mensaje = "Exactamente un identificador (email o usuario) es necesario para autenticar.",
                };

                throw e;
            }

            var usuario = await _contexto.Usuarios.SingleOrDefaultAsync(
                u => (u.NombreUsuario == infoUsuario.NombreUsuario || 
                        u.Email == infoUsuario.Email)
            );

            if (usuario is null) 
            {
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.USUARIO_NO_EXISTE,
                    Mensaje = "No existe el usuario con el identificador especificado.",
                };

                throw e;
            }

            if (!BCryptNet.Verify(infoUsuario.Password, usuario.Password))
            {
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.PASSWORD_INCORRECTO,
                    Mensaje = "La contraseña es incorrecta.",
                };

                throw e;  
            }

            var datosUsuario = usuario.ComoRespuestaToken();
            datosUsuario.Token = _generadorToken.Generar(usuario);
            return datosUsuario;
        }

        public Task EliminarUsuarioAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<DTOUsuario> GetUsuarioPorId(Guid id)
        {
            var usuario = await _contexto.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            return usuario.ComoDTO();
        }

        public async Task<List<DTOUsuario>> GetUsuariosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DTOUsuario> RegistrarAsync(DTOUsuario usuario)
        {
            usuario.Id = new Guid();

            if (await _contexto.Usuarios.AnyAsync(u => 
                u.NombreUsuario == usuario.NombreUsuario ||
                u.Email == usuario.Email ))
            {
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.USUARIO_EXISTE,
                    Mensaje = "Ya existe un usuario con esas credenciales.",
                };

                throw e; 
            }

            string hashContrasenia = BCryptNet.HashPassword(usuario.Password);
            var modeloUsuario = usuario.ComoModelo(hashContrasenia);

            _contexto.Usuarios.Add(modeloUsuario);
            await _contexto.SaveChangesAsync();

            return usuario;
        }
    }
}