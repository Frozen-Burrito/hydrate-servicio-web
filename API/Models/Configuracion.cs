using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos 
{
    [Table("Configuracion")]
    public class Configuracion 
    {
        public Guid Id { get; set; }

        public ThemeMode TemaDeColor { get; set; }

        public bool AportaDatosAbiertos { get; set; }

        public bool FormulariosRecurrentesActivados { get; set; }

        public bool IntegradoConGoogleFit { get; set; }

        public TiposDeNotificacion NotificacionesPermitidas { get; set; }

        [MaxLength(17)]
        public string IdDispositivo { get; set; }

        [MaxLength(10)]
        public string CodigoLocalizacion { get; set; }

        [ForeignKey("id_perfil")]
        public Perfil Perfil { get; set; }

        public static Configuracion PorDefecto() 
        {
            return new Configuracion
            {
                TemaDeColor = ThemeMode.SISTEMA,
                AportaDatosAbiertos = false,
                FormulariosRecurrentesActivados = false,
                IntegradoConGoogleFit = false,
                NotificacionesPermitidas = TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS,
                IdDispositivo = "",
                CodigoLocalizacion = "en-US",
            };
        }

        public DTOConfiguracion ComoDTO() 
        {
            return new DTOConfiguracion
            {
                Id = this.Id,
                TemaDeColor = (int) this.TemaDeColor,
                AportaDatosAbiertos = this.AportaDatosAbiertos,
                FormulariosRecurrentesActivados = this.FormulariosRecurrentesActivados,
                IntegradoConGoogleFit = this.IntegradoConGoogleFit,
                NotificacionesPermitidas = (int) this.NotificacionesPermitidas,
                IdDispositivo = this.IdDispositivo,
                CodigoLocalizacion = this.CodigoLocalizacion,
            };
        }
    
        public void Actualizar(DTOConfiguracion cambios) 
        {
            TemaDeColor = (Enums.ThemeMode) cambios.TemaDeColor;
            AportaDatosAbiertos = cambios.AportaDatosAbiertos;
            FormulariosRecurrentesActivados = cambios.FormulariosRecurrentesActivados;
            IntegradoConGoogleFit = cambios.IntegradoConGoogleFit;
            NotificacionesPermitidas = (Enums.TiposDeNotificacion) cambios.NotificacionesPermitidas;
            IdDispositivo = cambios.IdDispositivo;
            CodigoLocalizacion = cambios.CodigoLocalizacion;
        }
    }
}