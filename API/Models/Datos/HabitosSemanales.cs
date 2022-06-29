using System;
using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Modelos.Datos 
{
    public class HabitosSemanales 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        [Range(0, 24)]
        public double HorasDeSuenio { get; set; }

        [Range(0, 24)]
        public double HorasDeActividadFisica { get; set; }

        [Range(0, 24)]
        public double HorasDeOcupacion { get; set; }

        [Range(-60, 60)]
        public double TemperaturaMaxima { get; set; }

        public DateTime Fecha { get; set; }
    }
}