using System;
using System.Collections.Generic;

using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
	public class LlaveDeApi 
	{
		public int Id { get; private set; }

		public Guid IdUsuario { get; private set; }

		public Usuario Usuario { get; set; }

		public string Llave { get; private set; }

		public int PeticionesEnMes { get; private set; }

		public DateTime FechaDeCreacion { get; private set; }

		public RolDeUsuario RolDeAcceso { get; private set; }

		public LlaveDeApi(int id, Guid idUsuario, string llave, DateTime fechaDeCreacion)
		{
			Id = id;
			IdUsuario = idUsuario;
			Llave = llave;
			FechaDeCreacion = fechaDeCreacion;

			// Rol de acceso especifico para usuarios de llaves de API.
			RolDeAcceso = RolDeUsuario.TERCERO_CON_API_KEY;
		}

		public DTOLlaveDeAPI ComoDTO()
		{
			return new DTOLlaveDeAPI()
			{
				Id = this.Id,
				IdUsuario = this.IdUsuario,
				Llave = this.Llave,
				FechaDeCreacion = this.FechaDeCreacion.ToString("o"),
				PeticionesEnMes = this.PeticionesEnMes,
				RolDeAcceso = this.RolDeAcceso,
			};
		}
	}
}