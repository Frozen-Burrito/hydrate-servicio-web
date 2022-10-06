using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos 
{
    public class Pais 
    {
        public int Id { get; set; }

        public string Codigo { get; set; }

        public ICollection<Perfil> PerfilesQueResidenEnPais { get; set; }

        [NotMapped]
        private static Pais _paisNoEspecificado = new Pais
        {
            Id = 1,
            Codigo = "--",
        };

        public static Pais PaisNoEspecificado { get => _paisNoEspecificado; }

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