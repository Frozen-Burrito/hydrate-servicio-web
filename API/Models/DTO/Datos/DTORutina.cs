using System;
using System.Collections.Generic;
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

        public int Dias { get; set; }

        public string Hora { get; set; }

        public int IdActividad { get; set; }

        private static List<DiasDeLaSemana> diasDeLaSemana = new List<DiasDeLaSemana>{
            DiasDeLaSemana.LUNES,
            DiasDeLaSemana.MARTES,
            DiasDeLaSemana.MIERCOLES,
            DiasDeLaSemana.JUEVES,
            DiasDeLaSemana.VIERNES,
            DiasDeLaSemana.SABADO,
            DiasDeLaSemana.DOMINGO,
            DiasDeLaSemana.TODOS_LOS_DIAS,
        };

        public Rutina ComoNuevoModelo(Perfil perfil) 
        {
            TimeOnly hora;

            bool esHoraValida = TimeOnly.TryParse(this.Hora, out hora);

            if (!esHoraValida)
            {
                throw new FormatException("El valor de la hora no tiene el formato correcto.");
            }

            List<DiasDeLaSemana> dias = new List<DiasDeLaSemana>();

            foreach (DiasDeLaSemana dia in diasDeLaSemana) 
            {
                if ((dia & (DiasDeLaSemana)this.Dias) == dia) 
                {
                    dias.Add(dia);
                }
            }

            return new Rutina
            {
                Id = this.Id,
                IdPerfil = perfil.Id,
                PerfilDeUsuario = perfil,
                Dias = dias,
                Hora = hora,
                IdActividad = this.IdActividad,
            };
        }
    }
}