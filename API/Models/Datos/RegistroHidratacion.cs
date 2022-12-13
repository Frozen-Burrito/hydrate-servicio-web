using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using ServicioHydrate.Modelos.DTO.Datos; 

#nullable enable
namespace ServicioHydrate.Modelos.Datos
{
	public class RegistroDeHidratacion 
	{
		[Key]
		public int Id { get; set; }

		[Column("IdPerfil")]
		[ForeignKey("RegistrosDeHidratacion")]
        public int IdPerfil { get; set; }
        public Perfil? Perfil { get; set; }

		[Range(0, 5000)]
		public int CantidadEnMl { get; set; }

		[Range(0, 100)]
		public int PorcentajeCargaBateria { get; set; }

		[Range(-40.0, 75.0)]
		public double TemperaturaAproximada { get; set; }

		public DateTime Fecha { get; set; }

		public bool EsInformacionAbierta { get; set; }

		[NotMapped]
		public static int PorcentajeCargaBajo = 15;

		public DTORegistroDeHidratacion ComoDTO() 
		{
			return new DTORegistroDeHidratacion 
			{
				Id = this.Id,
				CantidadEnMl = this.CantidadEnMl,
                PorcentajeCargaBateria = this.PorcentajeCargaBateria,
                TemperaturaAproximada = this.TemperaturaAproximada,
                Fecha = this.Fecha.ToString("o"),
                IdPerfilUsuario = this.Perfil?.Id ?? -1,
			};
		}

		public void Actualizar(DTORegistroDeHidratacion cambios)
		{
			DateTime fecha;

            bool fechaEsValida = DateTime
                .TryParse(cambios.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!fechaEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

			CantidadEnMl = cambios.CantidadEnMl;
			PorcentajeCargaBateria = cambios.PorcentajeCargaBateria;
			TemperaturaAproximada = cambios.TemperaturaAproximada;
			Fecha = fecha;
		}
	}
}
#nullable disable
