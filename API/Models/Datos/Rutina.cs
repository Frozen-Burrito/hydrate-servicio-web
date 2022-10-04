using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.Datos 
{
    //TODO: revisar la estructura de este modelo, por ahora no es la mejor.
    public class Rutina 
    {
        public int Id { get; set; }

		[Column("id_perfil")]
        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        public int DiasDeOcurrencia { 
            get 
            {
                int bitmaskDias = 0b_0000_0000;

                foreach (var diaDeLaSemana in Dias) 
                {
                    bitmaskDias |= (int) diaDeLaSemana;
                }

                return bitmaskDias;
            }
        }

        [NotMapped]
        public List<DiasDeLaSemana> Dias { get; set; }

        public TimeOnly Hora { get; set; }

		[Column("id_actividad")]
        public int IdActividad { get; set; }
        public ActividadFisica RegistroDeActividad { get; set; }

        public DTORutina ComoDTO()
        {
            int bitsDias = 0;

            this.Dias.ForEach(dia => bitsDias |= (int) dia);

            return new DTORutina
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Dias = bitsDias,
                Hora = this.Hora.ToString("o"),
                IdActividad = this.IdActividad,
            };
        }
    }
}