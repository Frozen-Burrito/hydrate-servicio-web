using System;
using System.Collections.Generic;
using System.Globalization;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOPerfil 
	{
		public int Id { get; set; }

		public Guid IdCuentaUsuario { get; set; }

		public string Nombre { get; set; }
		public string Apellido { get; set; }

		public string FechaNacimiento { get; set; }

		public SexoUsuario SexoUsuario { get; set; }

		public double Estatura { get; set; }
		public double Peso { get; set; }

		public Ocupacion Ocupacion { get; set; }
		public CondicionMedica CondicionMedica { get; set; }

		public DTOPais PaisDeResidencia { get; set; }

		public int CantidadMonedas  { get; set; }
		public int NumModificaciones { get; set; }

		public string FechaSyncConGoogleFit { get; set; }
		public string FechaCreacion { get; set; }
		public string FechaModificacion { get; set; }

		public int IdEntornoSeleccionado { get; set; }
		public ICollection<int> IdsEntornosDesbloqueados { get; set; }

		public Perfil ComoNuevoModelo(Guid idCuentaUsuario, Pais paisDeResidencia) 
		{
			DateTime fechaCreacion;

            bool fechaCreacionEsValida = DateTime
                .TryParse(this.FechaCreacion, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaCreacion);

            if (!fechaCreacionEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }
			
			DateTime fechaModificacion;

            bool fechaModificacionEsValida = DateTime
                .TryParse(this.FechaModificacion, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaModificacion);

            if (!fechaModificacionEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

			DateTime fechaSyncConFit;

            bool fechaSyncConFitEsValida = DateTime
                .TryParse(this.FechaSyncConGoogleFit, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaSyncConFit);

            if (!fechaSyncConFitEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

			return new Perfil 
			{
				IdCuentaUsuario = idCuentaUsuario,
				Nombre = this.Nombre,
				Apellido = this.Apellido,
				FechaNacimiento = this.FechaNacimiento,
				SexoUsuario = this.SexoUsuario,
				Estatura = this.Estatura,
				Peso = this.Peso,
				Ocupacion = this.Ocupacion,
				PaisDeResidencia = paisDeResidencia,
				CantidadMonedas = 0,
				NumModificaciones = 0,
				IdEntornoSeleccionado = -1,
				EntornosDesbloqueados = new List<Entorno>(),
				FechaSyncConGoogleFit = fechaSyncConFit,
				FechaDeCreacion = fechaCreacion,
				FechaDeModificacion = fechaModificacion,
			};
		}
	}
}