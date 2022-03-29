using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    /// Un objeto de transferencia de datos para un Recurso Informativo.
    public class DTORecursoInformativo
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public string Titulo { get; set; }

        [Required]
        [Url]
        [MaxLength(64)]
        public string Url { get; set; }

        [MaxLength(300)]
        public string Descripcion { get; set; }

        [MaxLength(24)]
        public string FechaPublicacion { get; set; }
    }
}