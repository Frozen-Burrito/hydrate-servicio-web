using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;

#nullable enable
namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTONuevaActividad 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        [MaxLength(40)]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(33)]
        public string Fecha { get; set; } = string.Empty;

        /// La duración en minutos del registro de actividad física.
        [Range(1, 480)]
        public int Duracion { get; set; }

        /// La distancia recorrdia durante la actividad física, en kilómetros.
        [Range(0.001, 30.0)]
        public double Distancia { get; set; }

        [Range(0, 2500)]
        public int KcalQuemadas { get; set; }
    
        public bool AlAireLibre { get; set; }

        public int IdTipoDeActividad { get; set; }

        public bool EsRutina { get; set; }

        public RegistroDeActividad ComoNuevoModelo(
            TipoDeActividad datosDeActividad, 
            Rutina rutina,
            bool esParteDeDatosAbiertos = false
        ) 
        {
            DateTime fecha;

            bool strISO8601Valido = DateTime
                .TryParse(this.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!strISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            return new RegistroDeActividad
            {
                IdPerfil = this.IdPerfil,
                Titulo = this.Titulo,
                Fecha = fecha,
                Duracion = this.Duracion,
                Distancia = this.Distancia,
                KcalQuemadas = this.KcalQuemadas,
                FueAlAireLibre = this.AlAireLibre,
                TipoDeActividad = datosDeActividad,
                Rutina = rutina,
                EsInformacionAbierta = esParteDeDatosAbiertos,
            };
        }
    }
}
#nullable disable