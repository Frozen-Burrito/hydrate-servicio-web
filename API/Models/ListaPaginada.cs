using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServicioHydrate.Data 
{
	public class ListaPaginada<T> : List<T>
	{
		public int IndicePagina { get; private set; }
		public int PaginasTotales { get; private set; }

		public ListaPaginada()
		{
			IndicePagina = 1;
			PaginasTotales = 1;
		}

		public ListaPaginada(List<T> elementos, int total, int indicePagina, int sizePagina)
		{
			IndicePagina = indicePagina;
			PaginasTotales = (int) Math.Ceiling(total / (double) sizePagina);

			this.AddRange(elementos);
		}

		public bool TienePaginaAnterior => IndicePagina > 1;

		public bool TienePaginaSiguiente => IndicePagina < PaginasTotales;

		public static async Task<ListaPaginada<T>> CrearAsync(IQueryable<T> fuente, int indicePagina, int? sizePagina = 25)
		{
			var total = await fuente.CountAsync();

			int resultadosPorPagina = sizePagina ?? 25;

			int paginasTotales = (int) Math.Ceiling(total / (double) resultadosPorPagina);

			indicePagina = Math.Max(1, Math.Min(indicePagina, indicePagina));

			var elementos = await fuente.Skip((indicePagina - 1) * resultadosPorPagina)
										.Take(resultadosPorPagina).ToListAsync();

			return new ListaPaginada<T>(elementos, total, indicePagina, resultadosPorPagina);
		}
	}
}