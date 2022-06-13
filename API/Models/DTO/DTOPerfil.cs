using System;
using System.Collections.Generic;
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

		public int IdEntornoSeleccionado { get; set; }
		public ICollection<int> IdsEntornosDesbloqueados { get; set; }

		public Perfil ComoNuevoModelo(Guid idCuentaUsuario, Pais paisDeResidencia) 
		{
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
			};
		}
	}
}