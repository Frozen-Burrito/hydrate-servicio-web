using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Modelos.Datos 
{
    public class ReporteSemanal 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil Perfil { get; set; }

        [Range(0, 24)]
        public double HorasDeSuenio { get; set; }

        [Range(0, 24)]
        public double HorasDeActividadFisica { get; set; }

        [Range(0, 24)]
        public double HorasDeOcupacion { get; set; }

        [Range(-60, 60)]
        public double TemperaturaMaxima { get; set; }

        public DateTime Fecha { get; set; }

        public DTOReporteSemanal ComoDTO()
        {
            return new DTOReporteSemanal()
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                HorasDeSuenio = this.HorasDeSuenio,
                HorasDeActividadFisica = this.HorasDeActividadFisica,
                HorasDeOcupacion = this.HorasDeOcupacion,
                TemperaturaMaxima = this.TemperaturaMaxima,
                Fecha = this.Fecha.ToString("o"),
            };
        }
    }
}