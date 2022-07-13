using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServicioHydrate.Data;
using ServicioHydrate.Modelos;

#nullable enable
namespace ServicioHydrate.Autenticacion
{
	public class HandlerAuthLlaveDeAPI : AuthenticationHandler<OpcionesAuthLlaveDeAPI> 
	{
		private const string NombreHeaderDeLlaveAPI = "X-Api-Key";
		private const string NombreHeaderAutenticacionRegular = "Authorization";
		private const string ContentTypeDetallesProblema = "application/problem+json";

		// Repositorio de acceso a llaves de API almacenadas en BD.
		private readonly IServicioLlavesDeAPI _servicioLlavesDeAPI;

		private readonly ILogger<HandlerAuthLlaveDeAPI> _logger;

		public HandlerAuthLlaveDeAPI(
			IOptionsMonitor<OpcionesAuthLlaveDeAPI> opciones,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IServicioLlavesDeAPI servicioLlavesDeAPI
		) : base(opciones, logger, encoder, clock)
		{
			_servicioLlavesDeAPI = servicioLlavesDeAPI 
					?? throw new ArgumentNullException(nameof(servicioLlavesDeAPI));

			_logger = logger.CreateLogger<HandlerAuthLlaveDeAPI>();
		}

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
			bool existeHeaderApiKey = Request.Headers
				.TryGetValue(NombreHeaderDeLlaveAPI, out var valoresHeader);

			_logger.LogInformation($"Existe header de API key: {existeHeaderApiKey}");

            if (!existeHeaderApiKey)
			{
				// La peticion no incluye una llave de API. No producir 
				// resultado de autenticación.
				return AuthenticateResult.NoResult();
			}

			string? llaveProporcionada = valoresHeader.FirstOrDefault();

			_logger.LogInformation($"Llave proporcionada: {llaveProporcionada}");

			if (valoresHeader.Count == 0 || string.IsNullOrEmpty(llaveProporcionada))
			{
				// El header existe en la petición, pero no tiene un valor. No producir 
				// resultado de autenticación.
				return AuthenticateResult.NoResult();
			}

			LlaveDeApi? llaveRegistrada = await _servicioLlavesDeAPI.BuscarLlave(llaveProporcionada);

			_logger.LogInformation($"Llave registrada: {llaveRegistrada}");

			if (llaveRegistrada is null) 
			{
				// La llave tiene un valor incorrecto o no ha sido registrada.
				return AuthenticateResult.NoResult();
			} else 
			{
				// La llave de API tiene un valor y existe en la BD, intentar
				// autenticar con ella.
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Actor, llaveRegistrada.IdUsuario.ToString()),
					new Claim(ClaimTypes.Role, llaveRegistrada.RolDeAcceso.ToString()),
				};

				var identidad = new ClaimsIdentity(claims, Options.AuthenticationType);
				var identidades = new List<ClaimsIdentity>{ identidad };
				var principal = new ClaimsPrincipal(identidades);
				var ticket = new AuthenticationTicket(principal, Options.Scheme);

				return AuthenticateResult.Success(ticket);
			}
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
			bool hayOtroHeaderDeAutenticacion = Request.Headers
				.TryGetValue(NombreHeaderAutenticacionRegular, out var valoresHeader);

			_logger.LogInformation($"Hay otro header de autenticacion en request en HandleChallengeAsync(): {hayOtroHeaderDeAutenticacion}");

            if (!hayOtroHeaderDeAutenticacion)
			{
				// La llave de API es el único método de autenticación en la 
				// petición. Usarla para verificar la identidad.
				Response.StatusCode = 401;
				Response.ContentType = ContentTypeDetallesProblema;
				var detalles = new ProblemDetails();
				detalles.Detail = "No fue posible comprobar la identidad del usuario.";

				await Response.WriteAsync(JsonSerializer.Serialize(detalles));
			}
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
			Response.ContentType = ContentTypeDetallesProblema;
			var detalles = new ProblemDetails();
			detalles.Detail = "El usuario no posee el rol necesario para realizar esta acción.";

			await Response.WriteAsync(JsonSerializer.Serialize(detalles));
        }
    }
}
#nullable disable