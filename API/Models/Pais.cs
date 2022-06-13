using System.Collections.Generic;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos 
{
    public class Pais 
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public ICollection<Perfil> Perfiles { get; set; }

        public DTOPais ComoDTO() 
        {
            return new DTOPais
            {
                Id = this.Id,
                Codigo = this.Codigo
            };
        }
    }
}