using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOActividad 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        [MaxLength(40)]
        public string Titulo { get; set; }

        [MaxLength(33)]
        public string Fecha { get; set; }

        /// La duración en minutos del registro de actividad física.
        [Range(1, 480)]
        public int Duracion { get; set; }

        /// La distancia recorrdia durante la actividad física, en kilómetros.
        [Range(0.001, 30.0)]
        public double Distancia { get; set; }

        [Range(0, 2500)]
        public int KcalQuemadas { get; set; }
    
        public bool FueAlAireLibre { get; set; }

        public DTODatosActividad DatosDeActividad { get; set; }

        public DTORutina Rutina { get; set; }

        public bool EsInformacionAbierta { get; set; }

        public ActividadFisica ComoNuevoModelo(
            DatosDeActividad datosDeActividad, 
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

            return new ActividadFisica
            {
                IdPerfil = this.IdPerfil,
                Titulo = this.Titulo,
                Fecha = fecha,
                Duracion = this.Duracion,
                Distancia = this.Distancia,
                KcalQuemadas = this.KcalQuemadas,
                FueAlAireLibre = this.FueAlAireLibre,
                DatosActividad = datosDeActividad,
                Rutina = rutina,
                EsInformacionAbierta = esParteDeDatosAbiertos,
            };
        }
    }
}