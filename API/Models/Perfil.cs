using System;
using System.Linq;
using System.Collections.Generic;

using ServicioHydrate.Modelos.Enums;
using ServicioHydrate.Modelos.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ServicioHydrate.Modelos.Datos;
using System.Globalization;

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

		public int IdPaisDeResidencia { get; set; }
		[Required]
		public Pais PaisDeResidencia { get; set; }

		public int CantidadMonedas  { get; set; }
		public int NumModificaciones { get; set; }
		public DateTime? FechaSyncConGoogleFit { get; set; }
		public DateTime? FechaDeModificacion { get; set; }
		public DateTime FechaDeCreacion { get; set; }

		[ForeignKey("PerfilesQueSeleccionaron")]
		public int IdEntornoSeleccionado { get; set; }
		public Entorno EntornoSeleccionado { get; set; }

		public virtual ICollection<Entorno> EntornosDesbloqueados { get; set; }

		public virtual Configuracion Configuracion { get; set; }

		public virtual TokenFCM TokenFCM { get; set; }

		// Datos asociados al perfil.
		public virtual ICollection<RegistroDeActividad> RegistrosDeActFisica { get; set; }
		public virtual ICollection<DatosMedicos> RegistrosMedicos { get; set; }
		public virtual ICollection<Etiqueta> Etiquetas { get; set; }
		public virtual ICollection<ReporteSemanal> ReportesSemanales { get; set; }
		public virtual ICollection<MetaHidratacion> Metas { get; set; }
		public virtual ICollection<RegistroDeHidratacion> RegistrosDeHidratacion { get; set; }
		public virtual ICollection<Rutina> Rutinas { get; set; }

        public static Perfil PorDefecto(Guid? idCuentaUsuario) 
        {
            return new Perfil
            {
				IdCuentaUsuario = idCuentaUsuario ?? new Guid(),
				Nombre = "",
				Apellido = "",
				FechaNacimiento = DateTime.Now.ToString("o"),
				SexoUsuario = SexoUsuario.NO_ESPECIFICADO,
				Estatura = 0.0,
				Peso = 0.0,
				Ocupacion = Ocupacion.NO_ESPECIFICADO,
				IdPaisDeResidencia = Pais.PaisNoEspecificado.Id,
				CondicionMedica = CondicionMedica.NO_ESPECIFICADO,
				CantidadMonedas = 0,
				NumModificaciones = 0,
				IdEntornoSeleccionado = Entorno.PrimerEntornoDesbloqueado.Id,
				Configuracion = Configuracion.PorDefecto(),
				EntornosDesbloqueados = new List<Entorno>(),
				FechaSyncConGoogleFit = null,
				FechaDeCreacion = DateTime.Now,
				FechaDeModificacion = null,
            };
        }

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
				FechaSyncConGoogleFit = FechaSyncConGoogleFit?.ToString("o"),
				FechaCreacion = FechaDeCreacion.ToString("o"),
				FechaModificacion = FechaDeModificacion?.ToString("o"),
			};
		}

		public void Actualizar(DTOPerfilModificado cambiosAPerfil, ICollection<Entorno> entornos)
		{
			DateTime fechaSyncConFit;

            bool fechaSyncConFitEsValida = DateTime
                .TryParse(cambiosAPerfil.FechaSyncConGoogleFit, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaSyncConFit);

            if ((cambiosAPerfil.FechaSyncConGoogleFit is not null) && (!fechaSyncConFitEsValida))
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

			Nombre = cambiosAPerfil.Nombre;
			Apellido = cambiosAPerfil.Apellido;
			FechaNacimiento = cambiosAPerfil.FechaNacimiento;
			SexoUsuario = cambiosAPerfil.SexoUsuario;
			Estatura = cambiosAPerfil.Estatura;
			Peso = cambiosAPerfil.Peso;
			Ocupacion = cambiosAPerfil.Ocupacion;
			IdPaisDeResidencia = cambiosAPerfil.IdPaisDeResidencia;
			CondicionMedica = cambiosAPerfil.CondicionMedica;
			CantidadMonedas = cambiosAPerfil.CantidadMonedas;
			NumModificaciones = cambiosAPerfil.NumModificaciones;
			IdEntornoSeleccionado = cambiosAPerfil.IdEntornoSeleccionado;
			EntornosDesbloqueados = entornos.Where(entorno => cambiosAPerfil.IdsEntornosDesbloqueados.Contains(entorno.Id)).ToList();
			FechaDeCreacion = this.FechaDeCreacion;
			FechaSyncConGoogleFit = (cambiosAPerfil.FechaSyncConGoogleFit is not null) ? fechaSyncConFit : null;
			FechaDeModificacion = (NumModificaciones == 1) ? DateTime.Now : FechaDeModificacion;
		}
	}
}