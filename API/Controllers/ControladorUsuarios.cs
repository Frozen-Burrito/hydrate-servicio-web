using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ServicioHydrate.Data;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Modelos.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using ServicioHydrate.Modelos;
// using Newtonsoft.Json;

#nullable enable
namespace ServicioHydrate.Controladores 
{
    [ApiController]
    [Route("api/v1/usuarios")]
    [Authorize(Roles = "ADMINISTRADOR")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorUsuarios : ControllerBase
    {
        /// El repositorio de acceso a los Usuarios.
        private readonly IServicioUsuarios _repoUsuarios;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorUsuarios> _logger;

        public ControladorUsuarios(
            IServicioUsuarios servicioUsuarios,
            ILogger<ControladorUsuarios> logger)
        {
            // Asegurar que el controlador tenga una instancia del repositorio.
            if (servicioUsuarios is null) 
            {
                throw new ArgumentNullException(nameof(servicioUsuarios));
            }

            this._repoUsuarios = servicioUsuarios;
            this._logger = logger;
        }

        ///  
        /// <summary>
        /// Autentica un usuario utilizando sus credenciales de acceso. 
        /// </summary>
        /// <remarks>
        /// Si las credenciales son correctas y se logra autenticar al usuario,
        /// la respuesta a la petición tendrá el JWT.
        /// </remarks>
        /// <param name="datosUsuario">Las credenciales del usuario.</param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> LoginUsuario(DTOPeticionAutenticacion datosUsuario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try 
            {
                // Intentar autenticar el usuario con las credenciales recibidas.
                var resultado = await _repoUsuarios.AutenticarUsuario(datosUsuario);
                return Ok(resultado);
            }
            catch (ArgumentException e)
            {
                // Hay un error de autenticacion. Enviar una respuesta con 
                // código 401 (No autorizado).
                object? errDeAutenticacion = e.Data["ErrorAutenticacion"];

                MensajeErrorAutenticacion error = (errDeAutenticacion as MensajeErrorAutenticacion) 
                    ?? new MensajeErrorAutenticacion(); 

                return Unauthorized(error);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Crea una nueva cuenta de usuario con las credenciales recibidas.
        /// </summary>
        /// <remarks>
        /// Produce un error si ya existe una cuenta con el email o username especificados.
        /// Si las credenciales son correctas y se logra registrar y autenticar al usuario,
        /// la respuesta a la petición tendrá el JWT.
        /// </remarks>
        /// <param name="datosUsuario">Las credenciales de la nueva cuenta.</param>
        /// <returns></returns>
        [HttpPost("registro")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> RegistroUsuario(DTOPeticionAutenticacion datosUsuario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Intentar registrar una nueva cuenta de usuario con los datos.
                DTOUsuario usuario = await _repoUsuarios.RegistrarNuevoUsuario(datosUsuario);

                // Intentar autenticar al usuario usando los datos de la nueva cuenta.
                var resultado = await _repoUsuarios.AutenticarUsuario(
                    new DTOPeticionAutenticacion
                    {
                        NombreUsuario = datosUsuario.NombreUsuario,
                        Email = datosUsuario.Email,
                        Password = datosUsuario.Password
                    }
                );

                return Ok(resultado);
            }
            catch (ArgumentException e) 
            {
                // Hay un error de autenticacion. Enviar una respuesta con 
                // código 401 (No autorizado). 
                object? errDeAutenticacion = e.Data["ErrorAutenticacion"];

                MensajeErrorAutenticacion error = (errDeAutenticacion as MensajeErrorAutenticacion) 
                    ?? new MensajeErrorAutenticacion(); 

                return Unauthorized(error);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        //TODO: Eliminar este endpoint de pruebas.
        [HttpPost("test")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> CrearUsuarioDePruebas(
            [FromQuery] DTOModificarRol rolDeUsuario,
            [FromBody] DTOPeticionAutenticacion datosUsuario 
        )
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Intentar registrar una nueva cuenta de usuario con los datos.
                DTOUsuario usuario = await _repoUsuarios.RegistrarNuevoUsuario(datosUsuario);

                // Intentar autenticar al usuario usando los datos de la nueva cuenta.
                var resultado = await _repoUsuarios.AutenticarUsuario(
                    new DTOPeticionAutenticacion
                    {
                        NombreUsuario = datosUsuario.NombreUsuario,
                        Email = datosUsuario.Email,
                        Password = datosUsuario.Password
                    }
                );

                await _repoUsuarios.ModificarRolDeUsuario(usuario.Id, rolDeUsuario.NuevoRol);

                return Ok(resultado);
            }
            catch (ArgumentException e) 
            {
                // Hay un error de autenticacion. Enviar una respuesta con 
                // código 401 (No autorizado). 
                _logger.LogError(e, $"Error: {e}");

                object? errDeAutenticacion = e.Data["ErrorAutenticacion"];

                MensajeErrorAutenticacion error = (errDeAutenticacion as MensajeErrorAutenticacion) 
                    ?? new MensajeErrorAutenticacion(); 

                return Unauthorized(error);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene todas las cuentas de usuario registradas.
        /// </summary>
        /// <returns>Una colección con todas las cuentas de usuario.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOUsuario>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarios([FromQuery] DTOParamsPagina paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                var usuarios = await _repoUsuarios.GetUsuarios(paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOUsuario>
                                .DesdeColeccion(usuarios, numPagina, ruta);

                return Ok(resultado);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Obtiene los datos de usuario (nombre y correo) del usuario autenticado
        /// que hace la petición.
        /// </summary>
        /// <returns>El nombre de usuario y correo del usuario actual.</returns>
        [HttpGet("datos")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOUsuario))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsuarioPorId()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string strIdUsuario = this.User.Claims.FirstOrDefault(c => c.Type.Equals("id"))?.Value ?? "";
                Guid idUsuarioActual = new Guid(strIdUsuario);
                
                // Obtener los demás datos del usuario.
                DTOUsuario usuario = await _repoUsuarios.GetUsuarioPorId(idUsuarioActual);

                return Ok(usuario);
            }
            catch (ArgumentException e)
            {
                // No existe un usuario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Modifica el rol de un usuario, cambiando su nivel de autorizacion.
        /// </summary>
        /// <param name="idUsuario">El id del usuario a modificar.</param>
        /// <param name="nuevoRol">El nuevo rol de usuario.</param>
        /// <returns>204 - No Content</returns>
        [HttpPatch("{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ModificarRol(Guid idUsuario, DTOModificarRol modificacion)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "ruta no identificada";

            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                await _repoUsuarios.ModificarRolDeUsuario(idUsuario, modificacion.NuevoRol);

                return NoContent();
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }
    }
}
#nullable disable