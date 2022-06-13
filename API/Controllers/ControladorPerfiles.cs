using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Authorize]
    [Route("api/v1/perfiles")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorPerfiles : ControllerBase
    {
        /// El repositorio de acceso a los Perfiles de usuario.
        private readonly IServicioPerfil _repoPerfiles;

        private readonly IServicioUsuarios _repoUsuarios;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorPerfiles> _logger;

        public ControladorPerfiles(
            IServicioPerfil servicioPerfil,
            ILogger<ControladorPerfiles> logger
        )
        {
            // Asegurar que el controlador recibe una instancia del repositorio.
            if (servicioPerfil is null) 
            {
                throw new ArgumentNullException(nameof(servicioPerfil));
            }

            this._repoPerfiles = servicioPerfil;
            this._logger = logger;
        }

        /// <summary>
        /// Busca un perfil que identificado por el ID perfil de la URL y el ID de usuario 
        /// del JWT.
        /// </summary>
        /// <remarks>
        /// Siempre retorna el perfil del usuario autenticado. No es posible obtener 
        /// perfiles de usuarios ajenos.
        /// </remarks>
        /// <param name="idPerfil">El ID de perfil del perfil buscado.</param>
        /// <returns>El DTO del perfil encontrado, o un error.</returns>
        [HttpGet("{idPerfil}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOPerfil))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPerfilPorId(int idPerfil)
        {
            // Registrar un log de la peticion.
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);

                var perfil = await _repoPerfiles.GetPerfilPorId(idUsuarioActual, idPerfil);

                return Ok(perfil);
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario recibido en el JWT no es válido." + e.Message);
            }
            catch (ArgumentException e)
            {
                // No existe un perfil con el idPerfil e idUsuario especificados. Retorna 404.
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
        /// Registra un nuevo perfil asociado a la cuenta del usuario actual.
        /// </summary>
        /// <param name="datosPerfil">Los datos del nuevo perfil de usuario.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> RegistrarPerfilExistente([FromBody] DTOPerfil datosPerfil)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);

                DTOUsuario usuario = await _repoUsuarios.GetUsuarioPorId(idUsuarioActual);

                var perfil = await _repoPerfiles.RegistrarPerfilExistente(datosPerfil, usuario);

                return CreatedAtAction(
                    nameof(GetPerfilPorId),
                    new { idPerfil = perfil.Id },
                    perfil
                );
            }
            catch (FormatException e) 
            {
                _logger.LogInformation("Usuario no autorizado");
                return Unauthorized("El ID de usuario recibido en el JWT no es válido." + e.Message);
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Actualiza los datos de un perfil. 
        /// </summary>
        /// <remarks>
        /// Si hay datos con límite de modificaciones
        /// anuales y el perfil ya llegó a ese límite, el perfil no será actualizado.
        /// </remarks>
        /// <param name="idPerfil">El ID del perfil que se va a actualizar.</param>
        /// <param name="modificacionesPerfil">Los datos modificados del perfil</param>
        /// <returns>Resultado HTTP</returns>
        [HttpPatch("{idPerfil}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActualizarPerfil(int idPerfil, DTOPerfil modificacionesPerfil)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {                
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);

                modificacionesPerfil.Id = idPerfil;
                modificacionesPerfil.IdCuentaUsuario = idUsuarioActual;

                await _repoPerfiles.ActualizarPerfil(modificacionesPerfil);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe una orden con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (InvalidOperationException)
            {
                // Se solicito un cambio de estado no soportado. Retorna 405.
                return Problem(
                    "No es posible actualizar el estado de la orden con el nuevo estado recibido.",
                    statusCode: StatusCodes.Status405MethodNotAllowed
                );
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }    
        }

        /// <summary>
        /// Elimina un perfil de usuario.
        /// </summary>
        /// <remarks>
        /// Solo puede eliminarse el perfil del mismo usuario autenticado.
        /// </remarks>
        /// <param name="idPerfil">El ID del perfil que se va a eliminar.</param>
        /// <returns>Resultado HTTP</returns>
        [HttpDelete("{idPerfil}")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarPerfil(int idPerfil)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                // Obtener el ID del usuario actual desde el JWT.
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type == "id")?.Value ?? "";
                Guid idUsuarioActual = new Guid(idStr);

                await _repoPerfiles.EliminarPerfil(idUsuarioActual, idPerfil);

                return NoContent();
            }
            catch (ArgumentException e)
            {
                // No existe un comentario con el ID solicitado. Retorna 404.
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (DbUpdateException e)
            {
                // Hubo un error de base de datos. Enviarlo a los logs y retornar 503.
                _logger.LogError(e, $"Error de actualizacion de DB en {metodo} - {ruta}");

                return Problem(
                    "Ocurrió un error al procesar la petición. Intente más tarde.", 
                    statusCode: StatusCodes.Status503ServiceUnavailable
                );
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
