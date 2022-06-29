using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOEtiqueta 
    {
        public int Id { get; set; }

        [MaxLength(16)]
        public string Valor { get; set; }

        public int IdPerfil { get; set; }

        public Etiqueta ComoNuevoModelo()
        {
            return new Etiqueta
            {
                Valor = this.Valor,
                IdPerfil = this.IdPerfil,
            };
        }
    }
}