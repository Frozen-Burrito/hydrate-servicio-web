using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public class Entorno 
    {
        public int Id { get; set; }

        public string UrlImagen { get; set; }

        public int PrecioEnMonedas { get; set; }

        public DTOEntorno ComoDTO() 
        {
            return new DTOEntorno
            {
                Id = this.Id,
                UrlImagen = this.UrlImagen,
                PrecioEnMonedas = this.PrecioEnMonedas,
            };
        }
    }
}