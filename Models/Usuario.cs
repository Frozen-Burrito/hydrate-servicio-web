using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioHydrate.Modelos
{
    // Representa un Usuario en la base de datos.
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        // [MaxLength(32)]
        public string NombreUsuario { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [MaxLength(80)]
        public string Password { get; set; }

        public RolDeUsuario RolDeUsuario { get; set; }

        public virtual ICollection<Comentario> Comentarios { get; set; }
        public virtual ICollection<Respuesta> Respuestas { get; set; }
        
        public virtual ICollection<Comentario> ComentariosReportados { get; set; }
        public virtual ICollection<Comentario> ComentariosUtiles { get; set; }
        public virtual ICollection<Respuesta> RespuestasReportadas { get; set; }
        public virtual ICollection<Respuesta> RespuestasUtiles { get; set; }
    }
}