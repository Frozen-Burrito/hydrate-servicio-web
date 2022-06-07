using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO 
{
    public class DTOComentarioArchivado 
    {
        public int Id { get; set; }

        public int IdComentario { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Motivo { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }
    }
}