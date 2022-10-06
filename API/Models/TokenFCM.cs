using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos 
{
    [Table("TokensFCM")]
    public class TokenFCM 
    {
        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime TimestampGenerado { get; set; }

        public DateTime TimestampPersistido { get; set; }

        [Column("IdPerfil")]
        public int IdPerfil { get; set; }
        public Perfil Perfil { get; set; }

        public DTOTokenFCM ComoDTO() 
        {
            return new DTOTokenFCM
            {
                Token = this.Token,
                Timestamp = this.TimestampGenerado.ToString("o"),
            };
        }
    
        public void Actualizar(DTOTokenFCM cambios) 
        {
            var modeloActualizado = cambios.ComoNuevoModelo(IdPerfil);

            Token = modeloActualizado.Token;
            TimestampGenerado = modeloActualizado.TimestampGenerado;
            TimestampPersistido = modeloActualizado.TimestampPersistido;
        }
    } 
}