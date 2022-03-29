using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using ServicioHydrate.Data;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Modelos;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Utilidades;

namespace ServicioHydrate.Controladores 
{
    [Autorizacion]
    [ApiController]
    [Route("api/v1/usuarios")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ControladorUsuarios : ControllerBase
    {
        /// El repositorio de acceso a los Usuarios.
        private readonly IServicioUsuarios _repoUsuarios;

        // Permite generar Logs desde las acciones del controlador.
        private readonly ILogger<ControladorUsuarios> _logger;

        // Utilizado para obtener el secreto de los JWT.
        private readonly AppConfig _appConfig;

        public ControladorUsuarios(
            IServicioUsuarios servicioUsuarios,
            ILogger<ControladorUsuarios> logger,
            IOptions<AppConfig> appConfig )
        {
            // Asegurar que el controlador tenga una instancia del repositorio.
            if (servicioUsuarios is null) 
            {
                throw new ArgumentNullException(nameof(servicioUsuarios));
            }

            this._repoUsuarios = servicioUsuarios;
            this._logger = logger;
            this._appConfig = appConfig.Value;
        }

        /// Autentica un usuario utilizando sus credenciales de acceso. Produce un JWT.
        [PermitirAnonimo]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> LoginUsuario(DTOPeticionAutenticacion infoUsuario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try 
            {
                // Intentar autenticar el usuario con las credenciales recibidas.
                var resultado = await _repoUsuarios.AutenticarUsuario(infoUsuario);
                return Ok(resultado);
            }
            catch (ArgumentException e)
            {
                // Hay un error de autenticacion. Enviar una respuesta con 
                // código 401 (No autorizado).
                MensajeErrorAutenticacion error = e.Data["ErrorAutenticacion"] as MensajeErrorAutenticacion; 

                return Unauthorized(error);
            }
            catch (Exception e)
            {
                // Hubo un error inesperado. Enviarlo a los logs y retornar 500.
                _logger.LogError(e, $"Error no identificado en {metodo} - {ruta}");

                return Problem("Ocurrió un error al procesar la petición. Intente más tarde.");
            }
        }

        /// Recibe las credenciales de una nueva cuenta de usuario y la autentica.
        /// Produce un JWT. 
        [PermitirAnonimo]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> RegistroUsuario(DTOPeticionAutenticacion datosUsuario)
        {
            string strFecha = DateTime.Now.ToString("G");
            string metodo = Request.Method.ToString();
            string ruta = Request.Path.Value;
            _logger.LogInformation($"[{strFecha}] {metodo} - {ruta}");

            try 
            {
                // Intentar registrar una nueva cuenta de usuario con los datos.
                var usuario = await _repoUsuarios.RegistrarAsync(datosUsuario);

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
                MensajeErrorAutenticacion error = e.Data["ErrorAutenticacion"] as MensajeErrorAutenticacion; 

                return Unauthorized(error);
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