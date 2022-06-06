using System;
using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public class DTORespuesta
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Contenido { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }

        public bool Publicada { get; set; }

        public Guid IdAutor { get; set; }

        [MaxLength(129)]
        public String NombreAutor { get; set; }

        public int NumeroDeReportes { get; set; }
        public bool ReportadaPorUsuarioActual { get; set; }
        
        public int NumeroDeUtil { get; set; }
        public bool UtilParaUsuarioActual { get; set; }
    }
}