using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioHydrate.Modelos
{
    [Table("Respuestas")]    
    public class Respuesta
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Contenido { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }

        public bool Publicado { get; set; }

        public Usuario Autor { get; set; }

        public Comentario Comentario { get; set; }
        public int IdComentario { get; set; }

        public virtual ICollection<Usuario> ReportesDeUsuarios { get; set; }
        
        public virtual ICollection<Usuario> UtilParaUsuarios { get; set; }
    }
}