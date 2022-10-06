using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using ServicioHydrate.Modelos.DTO.Datos; 

#nullable enable
namespace ServicioHydrate.Modelos.Datos
{
    public class RegistroDeActividad 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil? Perfil { get; set; }

        [MaxLength(40)]
        public string Titulo { get; set; } = string.Empty;

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

        public int IdTipoDeActividad { get; set; }

        public TipoDeActividad? TipoDeActividad { get; set; }

        public Rutina? Rutina { get; set; }

        public bool EsInformacionAbierta { get; set; }

        [NotMapped]
        public bool EsActividadIntensa 
        {
            get 
            {
                bool duracionEsExtendida = Duracion > 60 * 3;
                bool distanciaEsGrande = Distancia > 10;
                bool kCalQuemadasEsIntensa = KcalQuemadas > 1500;

                return duracionEsExtendida || distanciaEsGrande || kCalQuemadasEsIntensa;
            }
        }

        public DTORegistroActividad ComoDTO() 
        {
            return new DTORegistroActividad
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Titulo = this.Titulo,
                Fecha = this.Fecha.ToString("o"),
                Duracion = this.Duracion,
                Distancia = this.Distancia,
                KcalQuemadas = this.KcalQuemadas,
                FueAlAireLibre = this.FueAlAireLibre,
                IdTipoDeActividad = this.IdTipoDeActividad,
                Rutina = this.Rutina?.ComoDTO(),
            };
        }

        public void Actualizar(DTORegistroActividad cambiosAlRegistro,  Rutina? rutinaActualizada)
		{
			DateTime fecha;

            bool fechaEsValida = DateTime
                .TryParse(cambiosAlRegistro.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!fechaEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            Titulo = cambiosAlRegistro.Titulo;
            Fecha = fecha;
            Duracion = cambiosAlRegistro.Duracion;
            Distancia = cambiosAlRegistro.Distancia;
            KcalQuemadas = cambiosAlRegistro.KcalQuemadas;
            FueAlAireLibre = cambiosAlRegistro.FueAlAireLibre;
            IdTipoDeActividad = cambiosAlRegistro.IdTipoDeActividad;
            Rutina = rutinaActualizada;
            EsInformacionAbierta = cambiosAlRegistro.EsInformacionAbierta;
		}
    }
}
#nullable disable