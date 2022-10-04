
using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOConfiguracion 
	{
        public Guid Id { get; set; }

        public int TemaDeColor { get; set; }

        public bool AportaDatosAbiertos { get; set; }

        public bool FormulariosRecurrentesActivados { get; set; }

        public bool IntegradoConGoogleFit { get; set; }

        public int NotificacionesPermitidas { get; set; }

        [MaxLength(17)]
        public string IdDispositivo { get; set; }

        [MaxLength(10)]
        public string CodigoLocalizacion { get; set; }

        public Configuracion ComoNuevoModelo() 
        {
            return new Configuracion
            {
                Id = new Guid(),
                TemaDeColor = (Enums.ThemeMode) this.TemaDeColor,
                AportaDatosAbiertos = this.AportaDatosAbiertos,
                FormulariosRecurrentesActivados = this.FormulariosRecurrentesActivados,
                IntegradoConGoogleFit = this.IntegradoConGoogleFit,
                NotificacionesPermitidas = (Enums.TiposDeNotificacion) this.NotificacionesPermitidas,
                IdDispositivo = this.IdDispositivo,
                CodigoLocalizacion = this.CodigoLocalizacion,
            };
        }
    }
}