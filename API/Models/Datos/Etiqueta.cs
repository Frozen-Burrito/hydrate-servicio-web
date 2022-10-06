using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Modelos.Datos
{
    public class Etiqueta
    {   
        [Key]
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil Perfil { get; set; }

        [MaxLength(16)]
        public string Valor { get; set; }

        public virtual ICollection<MetaHidratacion> Metas { get; set; }

        public DTOEtiqueta ComoDTO()
        {
            return new DTOEtiqueta
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Valor = this.Valor,
            };
        }
    }
}