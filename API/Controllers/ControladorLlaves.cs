using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using ServicioHydrate.Data;
using ServicioHydrate.Modelos.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ServicioHydrate.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServicioHydrate.Autenticacion;

#nullable enable
namespace ServicioHydrate.Controladores
{
    [ApiController]
    [Route("api/v1/llaves")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorLlaves : ControllerBase
    {
        /// El repositorio de acceso a las Ordenes.
        private readonly IServicioLlavesDeAPI _repoLlaves;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorLlaves> _logger;

        private const string EsquemaJwt = JwtBearerDefaults.AuthenticationScheme;
        private const string EsquemaApiKey = OpcionesAuthLlaveDeAPI.AuthenticationString;

        private const string EsquemaJwtAndApiKey = $"{EsquemaJwt},{EsquemaApiKey}";

        public ControladorLlaves(
            IServicioLlavesDeAPI servicioLlaves,
            ILogger<ControladorLlaves> logger
        )
        {
            this._repoLlaves = servicioLlaves ?? throw new ArgumentNullException(nameof(servicioLlaves));;
            this._logger = logger;
        }

        /// <summary>
        /// Retorna todas las llaves de API registradas.
        /// </summary>
        /// <returns>Resultado HTTP</returns>
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTOResultadoPaginado<DTOLlaveDeAPI>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTodasLasLlaves([FromQuery] DTOParamsPagina paramsPagina)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Obtener todas las llaves, segun los filtros recibidos.
                var llaves = await _repoLlaves.GetTodasLasLlaves(paramsPagina);

                int? numPagina = paramsPagina is not null ? paramsPagina.Pagina : 1;

                var resultado = DTOResultadoPaginado<DTOLlaveDeAPI>
                                .DesdeColeccion(llaves, numPagina, ruta);

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
        /// Retorna todas las llaves de API propias del usuario actual.
        /// </summary>
        /// <returns>Resultado HTTP</returns>
        [HttpGet("propias")]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ICollection<DTOLlaveDeAPI>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLlavesDelUsuario()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals("id"))?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                // Obtener todas las llaves del usuario.
                var llaves = await _repoLlaves.GetLlavesDeUsuario(idUsuario);

                _logger.LogInformation($"Llaves del usuario: {llaves.Count}");

                return Ok(llaves);
            }
            catch (FormatException e)
            {
                return BadRequest(e);
            }
            catch (Exception e) 
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// <summary>
        /// Genera una nueva llave de API.
        /// </summary>
        /// <returns>Resultado HTTP</returns>
        [HttpPost]
        [Authorize(Roles = "NINGUNO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerarLlaveDeAPI()
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals("id"))?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                if (idEsValido)
                {
                    // Intentar generar la nueva llave de API.
                    await _repoLlaves.GenerarNuevaLlave(idUsuario);

                    return NoContent();

                } else 
                {
                    return Unauthorized();
                }
            }
            catch (FormatException e)
            {
                return BadRequest(e);
            }
            catch (InvalidOperationException e)
            {
                // El usuario ya tiene tres llaves de API registradas.
                return Problem(
                    e.Message,
                    statusCode: StatusCodes.Status405MethodNotAllowed
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
        /// Intenta eliminar una llave de API.
        /// </summary>
        /// <param name="llave">El valor de la llave de API a eliminar.</param>
        /// <returns>Un resultado HTTP sin cuerpo, de confirmación de éxito.</returns>
        [HttpDelete("{llave}")]
        [Authorize(Roles = "NINGUNO,ADMINISTRADOR")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EliminarLlave([FromRoute] string llave, Guid? idPropietario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value ?? "/";
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try
            {
                string idStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals("id"))?.Value ?? String.Empty;
                bool idEsValido = Guid.TryParse(idStr, out Guid idUsuario);

                if (!idEsValido) 
                {
                    return Unauthorized();
                }

                Guid idUsuarioPropietario = idUsuario;

                // Revisar si la petición especifica un ID de propietario de llave que es
                // distinto al ID del usuario autenticado. Si es así, solo usuarios 
                // administradores pueden realizar la acción de eliminación.
                if (idPropietario is not null && !idUsuario.Equals(idPropietario))
                {
                    // Obtener el rol de usuario.
                    string rolStr = this.User.Claims.FirstOrDefault(i => i.Type.Equals(ClaimTypes.Role))?.Value ?? String.Empty;
                    bool rolEsValido = Enum.TryParse(rolStr, out RolDeUsuario rol);

                    if (rolEsValido && rol.Equals(RolDeUsuario.ADMINISTRADOR))
                    {
                        // Si el usuario es un administrador, usar el ID de usuario
                        // propietario especificado.
                        idUsuarioPropietario = (Guid) idPropietario;
                    }
                    else 
                    {
                        // Un usuario no administrador está intentando eliminar una llave ajena.
                        // No tiene permiso.
                        return Unauthorized();
                    }
                }

                // Eliminar la llave en BD.
                await _repoLlaves.EliminarLlave(idUsuario, llave);

                return NoContent();
            }
            catch (FormatException e)
            {
                return BadRequest(e.Message);
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
#nullable disable