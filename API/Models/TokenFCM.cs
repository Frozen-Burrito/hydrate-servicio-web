using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos 
{
    [Table("TokensFCM")]
    public class TokenFCM 
    {
        //TODO: Encontrar un primarykey adecuado para esta entidad.
        [Key]
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime TimestampGenerado { get; set; }

        public DateTime TimestampPersistido { get; set; }

        [ForeignKey("id_perfil")]
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
            var modeloActualizado = cambios.ComoNuevoModelo();

            Token = modeloActualizado.Token;
            TimestampGenerado = modeloActualizado.TimestampGenerado;
            TimestampPersistido = modeloActualizado.TimestampPersistido;
        }
    } 
}