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
                _preferenciasDeNotificaciones = 0b_0000_0000;

                foreach (var fuenteDeNotificaciones in _notificacionesPermitidas) 
                {
                    _preferenciasDeNotificaciones |= (int) fuenteDeNotificaciones;
                }

                return _preferenciasDeNotificaciones;
            } 
            set
            {
                _preferenciasDeNotificaciones = value;

                // Reiniciar las fuentes de notificaciones.
                _notificacionesPermitidas.Clear();

                if (_preferenciasDeNotificaciones == 0) 
                {
                    _notificacionesPermitidas.Add(TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS);
                } else 
                {
                    foreach (var fuente in fuentesDeNotificaciones)
                    {
                        int flagFuente = (int) fuente;
                        if ((flagFuente & _preferenciasDeNotificaciones) == flagFuente) 
                        {
                            _notificacionesPermitidas.Add(fuente);
                        }
                    }

                    bool todasLasNotifActivadas = _notificacionesPermitidas.Count == (fuentesDeNotificaciones.Count - 2);
                    bool todasEstaSeleccionado = _notificacionesPermitidas.Contains(TiposDeNotificacion.TODAS);
                    if (todasLasNotifActivadas && !todasEstaSeleccionado)
                    {
                        _notificacionesPermitidas.Add(TiposDeNotificacion.TODAS);
                    }
                }
            } 
        }

        [NotMapped]
        private List<TiposDeNotificacion> _notificacionesPermitidas = new List<TiposDeNotificacion>();

        [NotMapped]
        public List<TiposDeNotificacion> NotificacionesPermitidas 
        { 
            get 
            {
                // Reiniciar las fuentes de notificaciones.
                _notificacionesPermitidas.Clear();

                if (_preferenciasDeNotificaciones == 0) 
                {
                    _notificacionesPermitidas.Add(TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS);
                } else 
                {
                    foreach (var fuente in fuentesDeNotificaciones)
                    {
                        int flagFuente = (int) fuente;
                        if ((flagFuente & _preferenciasDeNotificaciones) == flagFuente) 
                        {
                            _notificacionesPermitidas.Add(fuente);
                        }
                    }

                    bool todasLasNotifActivadas = _notificacionesPermitidas.Count == (fuentesDeNotificaciones.Count - 2);
                    bool todasEstaSeleccionado = _notificacionesPermitidas.Contains(TiposDeNotificacion.TODAS);
                    if (todasLasNotifActivadas && !todasEstaSeleccionado)
                    {
                        _notificacionesPermitidas.Add(TiposDeNotificacion.TODAS);
                    }
                }

                return _notificacionesPermitidas;
            }
            set
            {
                _notificacionesPermitidas = value;
                _preferenciasDeNotificaciones = 0b_0000_0000;

                foreach (var fuenteDeNotificaciones in _notificacionesPermitidas) 
                {
                    _preferenciasDeNotificaciones |= (int) fuenteDeNotificaciones;
                }
            }
        }

        [MaxLength(17)]
        public string IdDispositivo { get; set; }

        [MaxLength(10)]
        public string CodigoLocalizacion { get; set; }

        public int IdPerfil { get; set; }
        public Perfil Perfil { get; set; }

        [NotMapped]
        public bool TieneNotificacionesActivadas 
        {
            get => !(NotificacionesPermitidas.Contains(TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS));
        }

        public bool PuedeRecibirNotificacionesDeFuente(TiposDeNotificacion fuenteDeNotificaciones) 
        {
            // Un usuario no puede recibir notificaciones si la fuente es NOTIFIACIONES_DESACTIVADAS.
            if (fuenteDeNotificaciones == TiposDeNotificacion.NOTIFIACIONES_DESACTIVADAS) 
            {
                return false;
            }

            return NotificacionesPermitidas.Contains(fuenteDeNotificaciones) || NotificacionesPermitidas.Contains(TiposDeNotificacion.TODAS);
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