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

		public ListaPaginada(List<T> elementos, int total, int indicePagina, int sizePagina)
		{
			IndicePagina = indicePagina;
			PaginasTotales = (int) Math.Ceiling(total / (double) sizePagina);

			this.AddRange(elementos);
		}

		public bool TienePaginaAnterior => IndicePagina > 1;

		public bool TienePaginaSiguiente => IndicePagina < PaginasTotales;

		public static async Task<ListaPaginada<T>> CrearAsync(IQueryable<T> fuente, int indicePagina, int sizePagina = 25)
		{
			var total = await fuente.CountAsync();
			var elementos = await fuente.Skip((indicePagina - 1) * sizePagina)
										.Take(sizePagina).ToListAsync();

			return new ListaPaginada<T>(elementos, total, indicePagina, sizePagina);
		}
	}
}