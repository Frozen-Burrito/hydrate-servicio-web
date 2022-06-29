using System;
using System.Linq;
using System.Collections.Generic;

using ServicioHydrate.Modelos.Enums;
using ServicioHydrate.Modelos.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos 
{
	public class Perfil 
	{
		public int Id { get; set; }

		public Guid IdCuentaUsuario { get; set; }

		[ForeignKey("IdCuentaUsuario")]
		public Usuario Cuenta { get; set; }

		public string Nombre { get; set; }
		public string Apellido { get; set; }

		[NotMapped]
		public string NombreCompleto { get => $"{Nombre} {Apellido}"; }

		public string FechaNacimiento { get; set; }

		public SexoUsuario SexoUsuario { get; set; }

		[Range(0.5, 3.5)]
		public double Estatura { get; set; }

		[Range(20, 200)]
		public double Peso { get; set; }

		public Ocupacion Ocupacion { get; set; }
		public CondicionMedica CondicionMedica { get; set; }

		public Pais PaisDeResidencia { get; set; }

		public int CantidadMonedas  { get; set; }
		public int NumModificaciones { get; set; }

		public int IdEntornoSeleccionado { get; set; }
		public ICollection<Entorno> EntornosDesbloqueados { get; set; }

		// Datos asociados al perfil.
		public virtual ICollection<ActividadFisica> RegistrosDeActFisica { get; set; }
		public virtual ICollection<DatosMedicos> RegistrosMedicos { get; set; }
		public virtual ICollection<Etiqueta> Etiquetas { get; set; }
		public virtual ICollection<HabitosSemanales> ReportesSemanales { get; set; }
		public virtual ICollection<Meta> Metas { get; set; }
		public virtual ICollection<RegistroDeHidratacion> RegistrosDeHidratacion { get; set; }
		public virtual ICollection<Rutina> Rutinas { get; set; }

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
				CondicionMedica = this.CondicionMedica,
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
			CondicionMedica = cambios.CondicionMedica;
			CantidadMonedas = cambios.CantidadMonedas;
			NumModificaciones = cambios.NumModificaciones;
			IdEntornoSeleccionado = cambios.IdEntornoSeleccionado;
			EntornosDesbloqueados = entornos;
		}
	}
}