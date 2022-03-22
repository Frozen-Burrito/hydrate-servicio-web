using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ServicioHydrate.Data;
using ServicioHydrate.Utilidades;

namespace ServicioHydrate.Autenticacion
{
    public class MiddlewareJWT
    {
        private readonly RequestDelegate _next;
        private readonly AppConfig _appConfig;

        public MiddlewareJWT(RequestDelegate next, IOptions<AppConfig> appConfig)
        {
            this._next = next;
            this._appConfig = appConfig.Value;
        }

        public async Task Invoke(HttpContext contexto, IServicioUsuarios servicioUsuarios)
        {
            var token = contexto.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await attachUserToContext(contexto, servicioUsuarios, token);
            }

            await _next(contexto);
        }

        private async Task attachUserToContext(HttpContext contexto, IServicioUsuarios servicioUsuarios, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appConfig.SecretoJWT);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validToken);

                var jwtToken = (JwtSecurityToken) validToken;
                var idUsuario = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                contexto.Items["Usuario"] = await servicioUsuarios.GetUsuarioPorId(idUsuario);
            }
            catch
            {

            }
        }
    }
}