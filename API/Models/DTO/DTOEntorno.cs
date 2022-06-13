
namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOEntorno 
	{
        public int Id { get; set; }

        public string UrlImagen { get; set; }

        public int PrecioEnMonedas { get; set; }

        public Entorno ComoNuevoModelo() 
        {
            return new Entorno
            {
                UrlImagen = this.UrlImagen,
                PrecioEnMonedas = this.PrecioEnMonedas,
            };
        }
    }
}