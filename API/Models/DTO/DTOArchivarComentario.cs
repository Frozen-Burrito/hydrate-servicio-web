using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO 
{
    public class DTOArchivarComentario 
    {        
        public int IdComentario { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Motivo { get; set; }
    }
}