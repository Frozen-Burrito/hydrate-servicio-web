using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOReporteSemanal 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        [Range(0, 24)]
        public double HorasDeSuenio { get; set; }

        [Range(0, 24)]
        public double HorasDeActividadFisica { get; set; }

        [Range(0, 24)]
        public double HorasDeOcupacion { get; set; }

        [Range(-60, 60)]
        public double TemperaturaMaxima { get; set; }

        [MaxLength(33)]
        public string Fecha { get; set; }

        public ReporteSemanal ComoNuevoModelo()
        {
            DateTime fecha;

            bool strISO8601Valido = DateTime
                .TryParse(this.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!strISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }
            
            return new ReporteSemanal()
            {
                IdPerfil = this.IdPerfil,
                HorasDeSuenio = this.HorasDeSuenio,
                HorasDeActividadFisica = this.HorasDeActividadFisica,
                HorasDeOcupacion = this.HorasDeOcupacion,
                TemperaturaMaxima = this.TemperaturaMaxima,
                Fecha = fecha,
            };
        }
    }
}