using System;
using System.Collections.Generic;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos 
{
	public class Perfil 
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

		public Pais PaisDeResidencia { get; set; }

		public int CantidadMonedas  { get; set; }
		public int NumModificaciones { get; set; }

		public int IdEntornoSeleccionado { get; set; }
		public ICollection<Entorno> EntornosDesbloqueados { get; set; }
	}
}