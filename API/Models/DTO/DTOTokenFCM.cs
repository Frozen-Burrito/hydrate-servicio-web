using System;
using System.Globalization;

namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOTokenFCM 
	{
        public string Token { get; set; }

        public string Timestamp { get; set; }

        public TokenFCM ComoNuevoModelo() 
        {
            DateTime timestamp;

            bool timestampEsValido = DateTime.TryParse(
                Timestamp, 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out timestamp
            );

            if (!timestampEsValido) 
            {
                throw new ArgumentException("El timestamp de generacion del token FCM no tiene formato ISO 8601");
            }

            return new TokenFCM
            {
                Token = Token,
                TimestampGenerado = timestamp,
                TimestampPersistido = DateTime.Now,
            };
        }
    }
}