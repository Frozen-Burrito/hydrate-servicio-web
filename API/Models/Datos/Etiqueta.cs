using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Modelos.Datos
{
    public class Etiqueta
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        [MaxLength(16)]
        public string Valor { get; set; }

        public virtual ICollection<Meta> Metas { get; set; }

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