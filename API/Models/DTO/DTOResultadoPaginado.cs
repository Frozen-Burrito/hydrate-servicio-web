using System;
using System.Collections.Generic;
using ServicioHydrate.Data;

#nullable enable
namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOResultadoPaginado<T> 
	{
		public int ResultadosPorPagina { get; set; }

		public int PaginaActual { get; set; }

		public int PaginasTotales { get; set; }

		public string? UrlPaginaSiguiente { get; set; }

		public string? UrlPaginaAnterior { get; set; }

		public ICollection<T> Resultados { get; set; }

		public DTOResultadoPaginado(
			int paginaActual,
			int paginasTotales, 
			string urlBase, 
			ICollection<T> resultados,
			int resultadosPorPagina = 25) 
		{
			PaginasTotales = Math.Max(paginasTotales, 1);

			PaginaActual = Math.Max(1, Math.Min(paginaActual, PaginasTotales));

			ResultadosPorPagina = Math.Max(resultadosPorPagina, 1);

			if (urlBase is not null) 
			{
				if (PaginaActual > 1) 
				{
					UrlPaginaAnterior = $"{urlBase}?pagina={PaginaActual - 1}";
				}

				if (PaginaActual < PaginasTotales)
				{
					UrlPaginaSiguiente = $"{urlBase}?pagina={PaginaActual + 1}";
				}
			}

			Resultados = resultados;
		}

		public static DTOResultadoPaginado<T> Vacio(string urlBase = "") 
		{
			return new DTOResultadoPaginado<T>(1, 1, urlBase, new List<T>());
		}

		public static DTOResultadoPaginado<T> DesdeColeccion(ICollection<T> coleccion, int? paginaActual = 1, string urlBase = "")
		{
			if (coleccion is ListaPaginada<T>)
			{
				var listaPaginada = coleccion as ListaPaginada<T>;

				return new DTOResultadoPaginado<T>(
					paginaActual ?? 1, 
					listaPaginada!.PaginasTotales, 
					urlBase, 
					listaPaginada,
					resultadosPorPagina: listaPaginada.Count
				);
			} else 
			{
				return new DTOResultadoPaginado<T>(1, 1, urlBase, coleccion);
			}
		}
	}
}
#nullable disable