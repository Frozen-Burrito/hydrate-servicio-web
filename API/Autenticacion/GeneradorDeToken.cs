using System;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ServicioHydrate.Modelos;
using ServicioHydrate.Utilidades;
using System.Collections.Generic;

namespace ServicioHydrate.Autenticacion
{
    public class GeneradorDeToken
    {
        public static String TipoClaimIdUsuario { get => "id"; }
        public static String TipoClaimIdPerfil { get => "idPerfil"; }

        private readonly AppConfig _appConfig;

        public GeneradorDeToken(IOptions<AppConfig> appConfig)
        {
            this._appConfig = appConfig.Value;
        }

        public string Generar(Usuario usuario, int idPerfilActivo)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(TipoClaimIdUsuario, usuario.Id.ToString()),
                new Claim(TipoClaimIdPerfil, idPerfilActivo.ToString()),
                new Claim(ClaimTypes.Role, usuario.RolDeUsuario.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.JwtSecret));

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: credenciales
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}