using System;
using System.Globalization;
using ServicioHydrate.Modelos;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTOUbicacion 
    {
        public int Id { get; set; }

        public int CodigoPostal { get; set; }

        public string NumeroExterior { get; set; }

        public string NumeroInterior { get; set; }

        public string Calle { get; set; }

        public string Colonia { get; set; }

        public string Ciudad { get; set; }

        public string Estado { get; set; }

        public DTOPais Pais { get; set; }

        public string FechaCreacion { get; set; }

        public Ubicacion ComoNuevoModelo(Pais pais) 
        {
            DateTime fechaDeCreacion;

            bool esStrISO8601Valido = DateTime
                .TryParse(this.FechaCreacion, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDeCreacion);

            if (!esStrISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601 para FechaCreacion, pero el string recibido no es válido");  
            }

            return new Ubicacion 
            {
                Id = this.Id,
                CodigoPostal = this.CodigoPostal,
                NumeroExterior = this.NumeroExterior,
                NumeroInterior = this.NumeroInterior,
                Calle = this.Calle,
                Colonia = this.Colonia,
                Ciudad = this.Ciudad,
                Estado = this.Estado,
                Pais = pais,
                FechaCreacion = fechaDeCreacion,
            };
        }
    }
}