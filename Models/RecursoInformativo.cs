using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos
{
    // Representa un Recurso Informativo en la base de datos.
    public class RecursoInformativo 
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

        [MaxLength(32)]
        [DataType("char")]
        public string FechaPublicacion { get; set; }
    }
}