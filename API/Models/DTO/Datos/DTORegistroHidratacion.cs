using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
	public class DTORegistroDeHidratacion 
	{
		public int Id { get; set; }

		public int CantidadEnMl { get; set; }

		public int PorcentajeCargaBateria { get; set; }

		public double TemperaturaAproximada { get; set; }

		[MaxLength(33)]
        [DataType("char")]
		public string Fecha { get; set; }

		public int IdPerfilUsuario { get; set; }

        public RegistroDeHidratacion ComoNuevoModelo(Perfil perfilDeUsuario, bool esParteDeDatosAbiertos = false) 
		{
            DateTime fecha;

            bool strISO8601Valido = DateTime
                .TryParse(this.Fecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!strISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

			int idPerfilAsociado = esParteDeDatosAbiertos ? Perfil.perfilServicio.Id : perfilDeUsuario.Id;

			var modelo = new RegistroDeHidratacion 
			{
				IdPerfil = idPerfilAsociado,
				CantidadEnMl = this.CantidadEnMl,
                PorcentajeCargaBateria = this.PorcentajeCargaBateria,
                TemperaturaAproximada = this.TemperaturaAproximada,
                Fecha = fecha,
				EsInformacionAbierta = esParteDeDatosAbiertos,
			};

			if (!esParteDeDatosAbiertos)
			{
				modelo.Id = this.Id;
			}

			return modelo;
		}
	}
}