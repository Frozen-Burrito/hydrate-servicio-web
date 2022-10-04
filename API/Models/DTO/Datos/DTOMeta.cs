
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOMeta 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        public PlazoTemporal Plazo { get; set; }

        public string FechaInicio { get; set; }
        public string FechaTermino { get; set; }

        [Range(0, 501)]
        public int RecompensaDeMonedas { get; set; }

        [Range(10, 5000)]
        public int CantidadEnMl { get; set; }

        [MaxLength(300)]
        public string Notas { get; set; }

        public ICollection<DTOEtiqueta> Etiquetas { get; set; }

        public Meta ComoNuevoModelo(ICollection<Etiqueta> etiquetas, Perfil perfil)
        {
            DateTime fechaDeInicio;

            bool esStrISO8601Valido = DateTime
                .TryParse(this.FechaInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDeInicio);

            if (!esStrISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601 para FechaInicio, pero el string recibido no es válido");  
            }

            DateTime fechaTermino;

            esStrISO8601Valido = DateTime
                .TryParse(this.FechaInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaTermino);

            if (!esStrISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601 para FechaInicio, pero el string recibido no es válido");  
            }

            return new Meta()
            {
                Id = this.Id,
                IdPerfil = perfil.Id,
                Plazo = this.Plazo,
                FechaInicio = fechaDeInicio,
                FechaTermino = fechaTermino,
                RecompensaDeMonedas = this.RecompensaDeMonedas,
                CantidadEnMl = this.CantidadEnMl,
                Notas = this.Notas,
                Etiquetas = etiquetas,
            };
        }
    }
}