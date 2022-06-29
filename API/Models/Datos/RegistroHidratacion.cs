using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ServicioHydrate.Modelos.DTO.Datos; 

namespace ServicioHydrate.Modelos.Datos
{
	public class RegistroDeHidratacion 
	{
		public int Id { get; set; }

		[Column("id_perfil")]
        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

		public int CantidadEnMl { get; set; }

		public int PorcentajeCargaBateria { get; set; }

		public double TemperaturaAproximada { get; set; }

		public DateTime Fecha { get; set; }

		public bool EsInformacionAbierta { get; set; }

		public DTORegistroDeHidratacion ComoDTO() 
		{
			return new DTORegistroDeHidratacion 
			{
				Id = this.Id,
				CantidadEnMl = this.CantidadEnMl,
                PorcentajeCargaBateria = this.PorcentajeCargaBateria,
                TemperaturaAproximada = this.TemperaturaAproximada,
                Fecha = this.Fecha.ToString("o"),
                IdPerfilUsuario = this.PerfilDeUsuario.Id,
			};
		}
	}
}
