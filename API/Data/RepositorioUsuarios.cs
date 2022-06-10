using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BCryptNet = BCrypt.Net.BCrypt;

using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Autenticacion;

#nullable enable
namespace ServicioHydrate.Data
{
    // Implementa todas las operaciones con Usuarios en la base de datos.
    public class RepositorioUsuarios : IServicioUsuarios
    {
        // El contexto de EF para la base de datos.
        private readonly ContextoDBSqlite _contexto;
        // Generador de JWT, utilizado por la autenticación. 
        private readonly GeneradorDeToken _generadorToken;

        public RepositorioUsuarios(ContextoDBSqlite contexto, GeneradorDeToken generadorToken)
        {
            this._contexto = contexto;
            this._generadorToken = generadorToken;
        }
        
        public async Task<DTOUsuario> ActualizarUsuario(DTOUsuario dtoUsuario)
        {
            // Buscar un usuario en la base de datos con el id recibido.
            var usuario = await _contexto.Usuarios.FindAsync(dtoUsuario.Id);

            if (usuario is null)
            {
                // No se encontró un usuario con el [id] solicitado.
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            usuario.NombreUsuario = dtoUsuario.NombreUsuario;
            usuario.Email = dtoUsuario.Email;

            _contexto.Entry(usuario).State = EntityState.Modified;

            await _contexto.SaveChangesAsync();

            return usuario.ComoDTO();
        }

        public async Task<DTORespuestaAutenticacion> AutenticarUsuario(DTOPeticionAutenticacion infoUsuario)
        {
            // Verificar que la petición tenga al menos un tipo de credencial de inicio
            // de sesión (hace falta el nombre de usuario O el email).
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

            // Buscar un usuario en la base de datos con el username o email recibidos.
            var usuario = await _contexto.Usuarios.SingleOrDefaultAsync(
                u => (u.NombreUsuario == infoUsuario.NombreUsuario || 
                      u.Email == infoUsuario.Email)
            );

            if (usuario is null) 
            {
                // Si el resultado de buscar un usuario en la base de datos es igual a null,
                // significa que no existe ninguna cuenta de usuario con el username o email
                // recibidos en el login. 
                // En este caso, lanzar una excepcion con tipo "USUARIO_NO_EXISTE".
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.USUARIO_NO_EXISTE,
                    Mensaje = "No existe el usuario con el identificador especificado.",
                };

                throw e;
            }

            // Verificar que los hashes de las contraseñas sean idénticos.
            if (!BCryptNet.Verify(infoUsuario.Password, usuario.Password))
            {
                // El hash de la contraseña recibida en la petición no es idéntico al
                // de la cuenta de usuario. Lanzar un error de "PASSWORD_INCORRECTO".
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.PASSWORD_INCORRECTO,
                    Mensaje = "La contraseña es incorrecta.",
                };

                throw e;  
            }

            // En este punto, la cuenta de usuario existe y las contraseñas coinciden.

            // Utilizando GeneradorDeToken.Generar, generar el JWT de autenticación para 
            // el usuario. Luego, retornar la RespuestaDeAutenticación.
            string jwt = _generadorToken.Generar(usuario);
            
            // Obtener un DTO de RespuestaDeAutenticación.
            var datosUsuario = new DTORespuestaAutenticacion{ Token = jwt };

            return datosUsuario;
        }

        public async Task EliminarUsuario(Guid id)
        {
            // Buscar un usuario en la base de datos con el id recibido.
            var usuario = await _contexto.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                // No se encontró un usuario con el [id] solicitado. Lanzar un 
                // error de argumento.
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            _contexto.Remove(usuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task ModificarRolDeUsuario(Guid idUsuario, RolDeUsuario nuevoRol)
        {
            // Buscar un usuario en la base de datos con el id recibido.
            var usuario = await _contexto.Usuarios.FindAsync(idUsuario);

            if (usuario is null)
            {
                // No se encontró un usuario con el [id] solicitado.
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            usuario.RolDeUsuario = nuevoRol;

            _contexto.Entry(usuario).State = EntityState.Modified;

            await _contexto.SaveChangesAsync();
        }

        public async Task<DTOUsuario> GetUsuarioPorId(Guid id)
        {
            // Buscar un usuario en la base de datos con el id recibido.
            var usuario = await _contexto.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                // No se encontró un usuario con el [id] solicitado. Lanzar un 
                // error de argumento.
                throw new ArgumentException("No existe un usuario con el ID especificado.");
            }

            // Retornar un DTO del usuario encontrado.
            return usuario.ComoDTO();
        }

        public async Task<ICollection<DTOUsuario>> GetUsuarios(DTOParamsPagina? paramsPagina)
        {
            if (_contexto.Usuarios.Count() == 0)
            {
                return new List<DTOUsuario>();
            }

            var usuarios = _contexto.Usuarios
                .Select(u => u.ComoDTO());

            var usuariosPaginados = await ListaPaginada<DTOUsuario>
                .CrearAsync(usuarios, paramsPagina?.Pagina ?? 1, paramsPagina?.SizePagina);

            return usuariosPaginados;
        }

        public async Task<DTOUsuario> RegistrarNuevoUsuario(DTOPeticionAutenticacion datosUsuario)
        {
            // Verificar que no exista un usuario en la base de datos con 
            // el username o email recibidos en la petición de registro.
            if (await _contexto.Usuarios.AnyAsync(u => 
                u.NombreUsuario == datosUsuario.NombreUsuario ||
                u.Email == datosUsuario.Email ))
            {
                // Existe un usuario en BD con una de las credenciales 
                // recibidas. Lanzar un error de autenticación de "USUARIO_EXISTE".
                var e = new ArgumentException("Authentication Error");
                e.Data["ErrorAutenticacion"] = new MensajeErrorAutenticacion
                {
                    Tipo = ErrorAutenticacion.USUARIO_EXISTE,
                    Mensaje = "Ya existe un usuario con esas credenciales.",
                };

                throw e; 
            }

            // Encriptar la contraseña encontrada en la petición de registro.
            string hashContrasenia = BCryptNet.HashPassword(datosUsuario.Password);

            // Crear un nuevo objeto Usuario, utilizando la contraseña encriptada.
            var modeloUsuario = datosUsuario.ComoModelo(hashContrasenia);

            // Generar un nuevo GUID para el usuario.
            modeloUsuario.Id = new Guid();

            // Agregar el nuevo objeto Usuario al contexto de la base de
            // datos. Luego, guardar los cambios al contexto.
            _contexto.Usuarios.Add(modeloUsuario);
            await _contexto.SaveChangesAsync();

            return modeloUsuario.ComoDTO();
        }
    }
}
#nullable disable
