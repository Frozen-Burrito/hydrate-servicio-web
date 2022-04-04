using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioHydrate.Modelos
{
    [Table("Comentarios")]
    public class Comentario
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

        public Usuario Autor { get; set; }

        // Los usuarios que han reportado este comentario.
        public virtual ICollection<Usuario> ReportesDeUsuarios { get; set; }
        
        // Los usuarios que han marcado este comentario como util.
        public virtual ICollection<Usuario> UtilParaUsuarios { get; set; }

        public virtual ICollection<Respuesta> Respuestas { get; set; }
    }
}