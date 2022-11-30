using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public class Entorno 
    {
        public int Id { get; set; }

        public string UrlImagen { get; set; }

        public int PrecioEnMonedas { get; set; }

        public virtual ICollection<Perfil> PerfilesQueSeleccionaron { get; set; }

        public virtual ICollection<Perfil> PerfilesQueDesbloquearon  {get; set; }

        [NotMapped]
        private static Entorno _primerEntornoDesbloqueado = new Entorno
        {
            Id = 1,
            UrlImagen = "1",
            PrecioEnMonedas = 0
        };

        public static Entorno PrimerEntornoDesbloqueado { get => _primerEntornoDesbloqueado; }

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