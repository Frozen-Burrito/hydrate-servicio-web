using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO 
{
    public class DTOArchivarComentario 
    {                
        [Required]
        [MaxLength(100)]
        public string Motivo { get; set; }
    }
}