using System;
using System.Collections.Generic;
using System.Globalization;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOPerfilModificado 
	{
		public int Id { get; set; }

		public Guid IdCuentaUsuario { get; set; }

		public string Nombre { get; set; } = "";
		public string Apellido { get; set; } = "";

		public string? FechaNacimiento { get; set; }

		public SexoUsuario SexoUsuario { get; set; }

		public double Estatura { get; set; }
		public double Peso { get; set; }

		public Ocupacion Ocupacion { get; set; }
		public CondicionMedica CondicionMedica { get; set; }

        public int IdPaisDeResidencia { get; set; }

		public int CantidadMonedas  { get; set; }
		public int NumModificaciones { get; set; }

		public string? FechaSyncConGoogleFit { get; set; }
		public string? FechaModificacion { get; set; }

		public int IdEntornoSeleccionado { get; set; }
		public ICollection<int> IdsEntornosDesbloqueados { get; set; } = new List<int>{ Entorno.PrimerEntornoDesbloqueado.Id };
	}
}
#nullable disable