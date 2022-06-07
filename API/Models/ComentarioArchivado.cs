using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioHydrate.Modelos
{
    [Table("ComentariosArchivados")]
    public class ComentarioArchivado
    {
        public int Id { get; set; }

        public int IdComentario { get; set; }

        public string Motivo { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }
    }
}
