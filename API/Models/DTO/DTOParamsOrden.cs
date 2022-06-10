using System;

#nullable enable
namespace ServicioHydrate.Modelos.DTO
{
	public class DTOParamsOrden
	{
		public Guid? IdOrden { get; set; }
		
		public EstadoOrden? Estado { get; set; }

		public DateTime? Desde { get; set; }

		public DateTime? Hasta { get; set; }

		public Guid? IdCliente { get; set; }

		public string? NombreCliente { get; set; }

		public string? EmailCliente { get; set; }
	}
}
#nullable disable