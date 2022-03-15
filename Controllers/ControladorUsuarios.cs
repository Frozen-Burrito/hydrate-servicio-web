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
        private readonly IServicioUsuarios _repoUsuarios;
        private readonly ILogger<ControladorUsuarios> _logger;
        private readonly AppConfig _appConfig;

        public ControladorUsuarios(
            IServicioUsuarios servicioUsuarios,
            ILogger<ControladorUsuarios> logger,
            IOptions<AppConfig> appConfig )
        {
            if (servicioUsuarios is null) 
            {
                throw new ArgumentNullException(nameof(servicioUsuarios));
            }

            this._repoUsuarios = servicioUsuarios;
            this._logger = logger;
            this._appConfig = appConfig.Value;
        }

        [PermitirAnonimo]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> LoginUsuario(DTOPeticionAutenticacion infoUsuario)
        {
            string logMsg = $"[{DateTime.UtcNow.ToLongTimeString()}] POST - /api/v1/usuarios/login";
            _logger.LogInformation(logMsg);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try 
            {
                var resultado = await _repoUsuarios.AutenticarUsuario(infoUsuario);
                return Ok(resultado);
            }
            catch (ArgumentException e)
            {
                MensajeErrorAutenticacion error = e.Data["ErrorAutenticacion"] as MensajeErrorAutenticacion; 

                return Unauthorized(error);
            }
        }

        [PermitirAnonimo]
        [HttpPost("registro")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DTORespuestaAutenticacion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesErrorResponseType(typeof(MensajeErrorAutenticacion))]
        public async Task<IActionResult> RegistroUsuario(DTOUsuario infoUsuario)
        {
            string logMsg = $"[{DateTime.UtcNow.ToLongTimeString()}] POST - /api/v1/usuarios/registro";
            _logger.LogInformation(logMsg);

            try 
            {
                var usuario = await _repoUsuarios.RegistrarAsync(infoUsuario);

                var resultado = await _repoUsuarios.AutenticarUsuario(
                    new DTOPeticionAutenticacion
                    {
                        NombreUsuario = infoUsuario.NombreUsuario,
                        Email = infoUsuario.Email,
                        Password = infoUsuario.Password
                    }
                );

                return Ok(resultado);
            }
            catch (ArgumentException e) 
            {
                MensajeErrorAutenticacion error = e.Data["ErrorAutenticacion"] as MensajeErrorAutenticacion; 

                return Unauthorized(error);
            }
        }
    }
}