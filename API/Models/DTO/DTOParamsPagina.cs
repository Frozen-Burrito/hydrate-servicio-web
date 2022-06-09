using System.ComponentModel.DataAnnotations;

#nullable enable
namespace ServicioHydrate.Modelos.DTO
{
	public class DTOParamsPagina 
	{
		public int? Pagina { get; set; }

		public int? SizePagina { get; set; }

		[MaxLength(128)]
		public string? Query { get; set; }
	}
}
#nullable disable