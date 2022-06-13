using System;
using System.Linq;
using System.Collections.Generic;

using ServicioHydrate.Modelos.Enums;
using ServicioHydrate.Modelos.DTO;

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

		public DTOPerfil ComoDTO() 
		{
			return new DTOPerfil 
			{
				Id = this.Id,
				IdCuentaUsuario = this.IdCuentaUsuario,
				Nombre = this.Nombre,
				Apellido = this.Apellido,
				FechaNacimiento = this.FechaNacimiento,
				SexoUsuario = this.SexoUsuario,
				Estatura = this.Estatura,
				Peso = this.Peso,
				Ocupacion = this.Ocupacion,
				PaisDeResidencia = this.PaisDeResidencia.ComoDTO(),
				CantidadMonedas = this.CantidadMonedas,
				NumModificaciones = this.NumModificaciones,
				IdEntornoSeleccionado = this.IdEntornoSeleccionado,
				IdsEntornosDesbloqueados = EntornosDesbloqueados.Select(e => e.Id).ToList(),
			};
		}

		public void Actualizar(DTOPerfil cambios, Pais paisModificado, ICollection<Entorno> entornos)
		{
			Nombre = cambios.Nombre;
			Apellido = cambios.Apellido;
			FechaNacimiento = cambios.FechaNacimiento;
			SexoUsuario = cambios.SexoUsuario;
			Estatura = cambios.Estatura;
			Peso = cambios.Peso;
			Ocupacion = cambios.Ocupacion;
			PaisDeResidencia = paisModificado;
			CantidadMonedas = cambios.CantidadMonedas;
			NumModificaciones = cambios.NumModificaciones;
			IdEntornoSeleccionado = cambios.IdEntornoSeleccionado;
			EntornosDesbloqueados = entornos;
		}
	}
}