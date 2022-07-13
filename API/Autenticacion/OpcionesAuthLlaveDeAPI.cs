using System;
using Microsoft.AspNetCore.Authentication;

namespace ServicioHydrate.Autenticacion
{
	public class OpcionesAuthLlaveDeAPI : AuthenticationSchemeOptions
	{
		public const string AuthenticationString = "API Key";
		public string Scheme => AuthenticationString;
		public string AuthenticationType => Scheme;
	}

	public static class AuthenticationBuilderExtensions 
	{
		public static AuthenticationBuilder AgregarSoporteParaLlaveDeAPI(
			this AuthenticationBuilder authBuilder,
			Action<OpcionesAuthLlaveDeAPI> opciones
		)
		{	
			return authBuilder.AddScheme<OpcionesAuthLlaveDeAPI, HandlerAuthLlaveDeAPI>(
				OpcionesAuthLlaveDeAPI.AuthenticationString, opciones
			);
		}

	}
}