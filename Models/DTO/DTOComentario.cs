using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    // Un objeto de transferencia de datos (DTO) para un Comentario.
    public class DTOComentario
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Asunto { get; set; }

        [Required]
        [MaxLength(300)]
        public string Contenido { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }

        public bool Publicado { get; set; }

        public Guid IdAutor { get; set; }

        // Los usuarios que han reportado este comentario.
        public int NumeroDeReportes { get; set; }
        public bool ReportadoPorUsuarioActual { get; set; }
        
        // Los usuarios que han marcado este comentario como util.
        public int NumeroDeUtil { get; set; }
        public bool UtilParaUsuarioActual { get; set; }
    }
}