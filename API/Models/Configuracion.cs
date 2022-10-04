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

        [NotMapped]
        private int _preferenciasDeNotificaciones;

        public int PreferenciasDeNotificaciones 
        { 
            get
            {
                int bitmaskFuentesNotif = 0b_0000_0000;

                foreach (var fuenteDeNotificaciones in NotificacionesPermitidas) 
                {
                    bitmaskFuentesNotif |= (int) fuenteDeNotificaciones;
                }

                _preferenciasDeNotificaciones = bitmaskFuentesNotif;

                return bitmaskFuentesNotif;
            } 
            set
            {
                // Reiniciar las fuentes de notificaciones.
                NotificacionesPermitidas.Clear();

                if (value == 0) 
                {
                    _preferenciasDeNotificaciones = 0;
                    NotificacionesPermitidas.Add(TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS);
                } else 
                {
                    foreach (var fuente in fuentesDeNotificaciones)
                    {
                        if (((int)fuente & value) == (int)fuente) 
                        {
                            NotificacionesPermitidas.Add(fuente);
                        }
                    }
                }
            } 
        }

        [NotMapped]
        public List<TiposDeNotificacion> NotificacionesPermitidas { get; set; }

        [MaxLength(17)]
        public string IdDispositivo { get; set; }

        [MaxLength(10)]
        public string CodigoLocalizacion { get; set; }

        [ForeignKey("id_perfil")]
        public Perfil Perfil { get; set; }

        [NotMapped]
        public bool TieneNotificacionesActivadas 
        {
            get => !(NotificacionesPermitidas.Contains(TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS));
        }

        private static List<TiposDeNotificacion> fuentesDeNotificaciones = new List<TiposDeNotificacion>{
            TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS,
            TiposDeNotificacion.RECORDATORIOS_METAS,
            TiposDeNotificacion.ALERTAS_BATERIA_DISPOSITIVO,
            TiposDeNotificacion.RECORDATORIOS_RUTINA,
            TiposDeNotificacion.RECORDATORIOS_DESCANSO,
            TiposDeNotificacion.TODAS,
        };

        public static Configuracion PorDefecto() 
        {
            return new Configuracion
            {
                TemaDeColor = ThemeMode.SISTEMA,
                AportaDatosAbiertos = false,
                FormulariosRecurrentesActivados = false,
                IntegradoConGoogleFit = false,
                NotificacionesPermitidas = new List<TiposDeNotificacion>() 
                { 
                    TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS, 
                },
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
                NotificacionesPermitidas = _preferenciasDeNotificaciones,
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
            PreferenciasDeNotificaciones = cambios.NotificacionesPermitidas;
            IdDispositivo = cambios.IdDispositivo;
            CodigoLocalizacion = cambios.CodigoLocalizacion;
        }
    }
}