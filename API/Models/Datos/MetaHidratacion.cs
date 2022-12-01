using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Modelos.Datos 
{
    public class MetaHidratacion 
    {
        public int Id { get; set; }
        
        public int IdPerfil { get; set; }
        public Perfil? Perfil { get; set; }

        public PlazoTemporal Plazo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaTermino { get; set; }

        [Column("RecomensaMonedas")]
        [Range(0, 501)]
        public int RecompensaDeMonedas { get; set; }

        [Column("CantidadMl")]
        [Range(10, 5000)]
        public int CantidadEnMl { get; set; }

        [MaxLength(300)]
        public string Notas { get; set; } = "";

        public DateTime FechaCreacion { get; set; }

        public virtual ICollection<Etiqueta> Etiquetas { get; set; } = new List<Etiqueta>();

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

        public void Actualizar(DTOMeta cambiosEnMeta, List<Etiqueta> etiquetasDelPerifl, Perfil perfil)
		{
			DateTime fechaDeInicio;

            bool fechaDeInicioEsValida = DateTime
                .TryParse(cambiosEnMeta.FechaInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDeInicio);

            if (!fechaDeInicioEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            DateTime fechaDeTermino;

            bool fechaDeTerminoEsValida = DateTime
                .TryParse(cambiosEnMeta.FechaTermino, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDeTermino);

            if (!fechaDeTerminoEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            Plazo = cambiosEnMeta.Plazo;
            FechaInicio = fechaDeInicio;
            FechaTermino = fechaDeTermino;
            RecompensaDeMonedas = cambiosEnMeta.RecompensaDeMonedas;
            CantidadEnMl = cambiosEnMeta.CantidadEnMl;
            Notas = cambiosEnMeta.Notas;

            List<Etiqueta> etiquetasModificadas = new List<Etiqueta>();

            foreach (var etiqueta in cambiosEnMeta.Etiquetas) 
            {
                Etiqueta? etiquetaYaExistente = etiquetasDelPerifl.Count > 0 
                    ?  etiquetasDelPerifl.Where(e => e.Id.Equals(etiqueta.Id)).FirstOrDefault()
                    : null;

                if (etiquetaYaExistente is null) 
                {
                    etiquetaYaExistente = etiqueta.ComoNuevoModelo(perfil);
                } else 
                {
                    etiquetaYaExistente.Actualizar(cambios: etiqueta);
                }

                etiquetasModificadas.Add(etiquetaYaExistente);
            }

            Etiquetas = etiquetasModificadas;
		}
    }
}
#nullable disable
