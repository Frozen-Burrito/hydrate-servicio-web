using System;
using System.ComponentModel.DataAnnotations.Schema;

using ServicioHydrate.Modelos.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTORutina
    {
        [NotMapped]
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        public DiasDeLaSemana Dias { get; set; }

        public string Hora { get; set; }

        public int IdActividad { get; set; }

        public Rutina ComoNuevoModelo() 
        {
            TimeOnly hora;

            bool esHoraValida = TimeOnly.TryParse(this.Hora, out hora);

            if (!esHoraValida)
            {
                throw new FormatException("El valor de la hora no tiene el formato correcto.");
            }

            return new Rutina
            {
                IdPerfil = this.IdPerfil,
                Dias = this.Dias,
                Hora = hora,
                IdActividad = this.IdActividad,
            };
        }
    }
}