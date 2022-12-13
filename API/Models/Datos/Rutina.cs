using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.Datos 
{
    //TODO: revisar la estructura de este modelo, por ahora no es la mejor.
    public class Rutina 
    {
        public int Id { get; set; }

		[Column("IdPerfil")]
        public int IdPerfil { get; set; }
        public Perfil Perfil { get; set; }

        [NotMapped]
        private int _diasDeRutina;

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
            set
            {
                // Reiniciar los días de ocurrencia de la rutina.
                Dias.Clear();

                _diasDeRutina = value;

                if (_diasDeRutina > 0) 
                {
                    foreach (var diaDeLaSemana in diasDeLaSemana)
                    {
                        if (((int)diaDeLaSemana & _diasDeRutina) == (int)diaDeLaSemana) 
                        {
                            Dias.Add(diaDeLaSemana);
                        }
                    }
                }
            }
        }

        [NotMapped]
        public List<DiasDeLaSemana> Dias { get; set; } = new List<DiasDeLaSemana>();

        public TimeOnly Hora { get; set; }

		[Column("IdRegistroActividad")]
        public int IdActividad { get; set; }
        public RegistroDeActividad RegistroDeActividad { get; set; }

        public DateTime FechaCreacion { get; set; }

        [NotMapped]
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

        public void Actualizar(DTORutina cambiosEnRutina)
		{
			TimeOnly horaDeRutina;

            bool horaEsValida = TimeOnly
                .TryParse(cambiosEnRutina.Hora, out horaDeRutina);

            if (!horaEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            List<DiasDeLaSemana> diasDondeOcurreRutina = new List<DiasDeLaSemana>();

            if (cambiosEnRutina.Dias > 0) 
            {
                foreach (var diaDeLaSemana in diasDeLaSemana)
                {
                    if (((int) diaDeLaSemana & cambiosEnRutina.Dias) == (int) diaDeLaSemana) 
                    {
                        diasDondeOcurreRutina.Add(diaDeLaSemana);
                    }
                }
            }

            Dias = diasDondeOcurreRutina;
            Hora = horaDeRutina;
            IdActividad = cambiosEnRutina.IdActividad;
		}
    }
}