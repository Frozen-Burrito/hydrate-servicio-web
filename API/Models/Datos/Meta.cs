using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.Datos 
{
    public class Meta 
    {
        public int Id { get; set; }

        [Column("id_perfil")]
        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        public PlazoTemporal Plazo { get; set; }

        public DateTime FechaInicio { get; set; }

        [Column("fecha_fin")]
        public DateTime FechaTermino { get; set; }

        [Column("recompensa")]
        [Range(0, 501)]
        public int RecompensaDeMonedas { get; set; }

        [Column("cantidad")]
        [Range(10, 5000)]
        public int CantidadEnMl { get; set; }

        [MaxLength(300)]
        public string Notas { get; set; }

        public virtual ICollection<Etiqueta> Etiquetas { get; set; }

        public DTOMeta ComoDTO() 
        {
            return new DTOMeta()
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Plazo = this.Plazo,
                FechaInicio = this.FechaInicio.ToString("o"),
                FechaTermino = this.FechaTermino.ToString("o"),
                RecompensaDeMonedas = this.RecompensaDeMonedas,
                CantidadEnMl = this.CantidadEnMl,
                Notas = this.Notas,
                Etiquetas = this.Etiquetas.Select(e => e.ComoDTO()).ToList(),
            };
        }
    }
}
