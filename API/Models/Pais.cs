using System.Collections.Generic;

namespace ServicioHydrate.Modelos 
{
    public class Pais 
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public ICollection<Perfil> Perfiles { get; set; }
    }
}