using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ServicioHydrate.Modelos;
using ServicioHydrate.Utilidades;

namespace ServicioHydrate.Autenticacion
{
    public class GeneradorDeToken
    {
        private readonly AppConfig _appConfig;

        public GeneradorDeToken(IOptions<AppConfig> appConfig)
        {
            this._appConfig = appConfig.Value;
        }

        public string Generar(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appConfig.SecretoJWT);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", usuario.Id.ToString())}),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}