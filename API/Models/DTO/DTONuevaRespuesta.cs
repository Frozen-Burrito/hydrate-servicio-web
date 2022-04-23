using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    // Un objeto de transferencia de datos (DTO) para la 
    // creación de una Respuesta.
    public class DTONuevaRespuesta
    {
        [Required]
        [MaxLength(300)]
        public string Contenido { get; set; }
    }
}