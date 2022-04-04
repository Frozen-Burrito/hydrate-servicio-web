using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    // Un objeto de transferencia de datos (DTO) para la 
    // creación de un Comentario.
    public class DTONuevoComentario
    {
        [Required]
        [MaxLength(60)]
        public string Asunto { get; set; }

        [Required]
        [MaxLength(300)]
        public string Contenido { get; set; }
    }
}