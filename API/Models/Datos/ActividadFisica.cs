using System;
using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.DTO.Datos; 

namespace ServicioHydrate.Modelos.Datos
{
    public class ActividadFisica 
    {
        public int Id { get; set; }

        [Required]
        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        [MaxLength(40)]
        public string Titulo { get; set; }

        public DateTime Fecha { get; set; }

        /// La duración en minutos del registro de actividad física.
        [Range(1, 480)]
        public int Duracion { get; set; }

        /// La distancia recorrdia durante la actividad física, en kilómetros.
        [Range(0.001, 30.0)]
        public double Distancia { get; set; }

        [Range(0, 2500)]
        public int KcalQuemadas { get; set; }
    
        public bool FueAlAireLibre { get; set; }

        public DatosDeActividad DatosActividad { get; set; }

        public Rutina Rutina { get; set; }

        public bool EsInformacionAbierta { get; set; }

        public DTOActividad ComoDTO() 
        {
            return new DTOActividad
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Titulo = this.Titulo,
                Fecha = this.Fecha.ToString("o"),
                Duracion = this.Duracion,
                Distancia = this.Distancia,
                KcalQuemadas = this.KcalQuemadas,
                FueAlAireLibre = this.FueAlAireLibre,
                DatosDeActividad = this.DatosActividad.ComoDTO(),
                Rutina = this.Rutina.ComoDTO(),
            };
        }
    }
}