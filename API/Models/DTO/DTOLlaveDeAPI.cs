using System;
using System.Collections.Generic;

namespace ServicioHydrate.Modelos.DTO
{
	public class DTOLlaveDeAPI
	{
		public int Id { get; set; }

		public Guid IdUsuario { get; set; }

		public string Nombre { get; set; }

		public string Llave { get; set; }

		public int PeticionesEnMes { get; set; }

		public string FechaDeCreacion { get; set; }

		public RolDeUsuario RolDeAcceso { get; set; }
	}
}