using System;

using ServicioHydrate.Modelos.DTO;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos
{
	public class LlaveDeApi 
	{
		public int Id { get; set; }

		public Guid IdUsuario { get; set; }

		public Usuario Usuario { get; set; }

		public string Llave { get; set; }

		public string NombreDelCliente { get; set; }

		public int PeticionesEnMes { get; set; }

		public int ErroresEnMes { get; set; }

		public DateTime FechaDeCreacion { get; set; }

		public RolDeUsuario RolDeAcceso { get; set; }

		public bool TuvoActividadEnMesPasado => PeticionesEnMes > 0;

		public LlaveDeApi()
		{
		}

		public LlaveDeApi(int id, Guid idUsuario, string nombreDelCliente, string llave, DateTime fechaDeCreacion)
		{
			Id = id;
			IdUsuario = idUsuario;
			NombreDelCliente = nombreDelCliente;
			Llave = llave;
			FechaDeCreacion = fechaDeCreacion;

			// Rol de acceso especifico para usuarios de llaves de API.
			RolDeAcceso = RolDeUsuario.TERCERO_CON_API_KEY;
		}

		public void RegenerarLlave()
		{
			Llave = Guid.NewGuid().ToString();
		}

		public void RegistrarUso()
		{
			PeticionesEnMes += 1;
		}

		public void RegistrarError()
		{
			ErroresEnMes += 1;
		}

		public DTOLlaveDeAPI ComoDTO()
		{
			return new DTOLlaveDeAPI()
			{
				Id = this.Id,
				IdUsuario = this.IdUsuario,
				Nombre = this.NombreDelCliente,
				Llave = this.Llave,
				FechaDeCreacion = this.FechaDeCreacion.ToString("o"),
				PeticionesEnMes = this.PeticionesEnMes,
				RolDeAcceso = this.RolDeAcceso,
			};
		}
		
		public DTOLlaveDeAPIAdmin ComoDTOAdmin()
		{
			return new DTOLlaveDeAPIAdmin()
			{
				Id = this.Id,
				IdPropietario = this.IdUsuario,
				NombrePropietario = this.Usuario.NombreUsuario,
				EmailPropietario = this.Usuario.Email,
				NombreDelCliente = this.NombreDelCliente,
				NumeroDePeticiones = this.PeticionesEnMes,
				TuvoActividadEnMesPasado = this.TuvoActividadEnMesPasado,
			};
		}
	}
}