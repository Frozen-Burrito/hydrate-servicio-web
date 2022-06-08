
namespace ServicioHydrate.Utilidades 
{
	public interface IServicioFiltroContenido 
	{
		bool ContenidoEsApto(string contenido);

		bool ContenidoIncluyePalabrasClave(string contenido);
	}
}