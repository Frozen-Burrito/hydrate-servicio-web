using System.Linq;
using System.Collections.Generic;

namespace ServicioHydrate.Utilidades
{
	public class FiltroDeComentarios : IServicioFiltroContenido
	{
		public bool ContenidoEsApto(string contenido)
		{
			List<string> palabrasMalas = new List<string>(){"tonto", "basura", "retrasado"};
			int menorLongitud = palabrasMalas.Min(p => p.Length);

			List<string> palabras = contenido.Split(" ").ToList();

			foreach (string palabra in palabras)
			{
				if (palabra.Length >= menorLongitud && palabrasMalas.Contains(palabra))
				{
					return false;
				}
			}

			return true;
		}

		public bool ContenidoIncluyePalabrasClave(string contenido)
		{
			List<string> keywords = new List<string>(){"agua", "hidratación", "botella", "app"};
			int menorLongitud = keywords.Min(p => p.Length);

			List<string> palabras = contenido.Split(" ").ToList();

			foreach (string palabra in palabras)
			{
				if (palabra.Length >= menorLongitud && keywords.Contains(palabra))
				{
					return true;
				}
			}

			return false;
		}
	}
}