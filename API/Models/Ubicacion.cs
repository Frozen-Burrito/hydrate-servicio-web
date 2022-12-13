using System;
using ServicioHydrate.Modelos.DTO;

namespace ServicioHydrate.Modelos
{
    public class Ubicacion
    {
        public int Id { get; set; }

        public Guid IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public int CodigoPostal { get; set; }

        public string NumeroExterior { get; set; }

        public string NumeroInterior { get; set; }

        public string Calle { get; set; }

        public string Colonia { get; set; }

        public string Ciudad { get; set; }

        public string Estado { get; set; }

        public int IdPais { get; set; }
        public Pais Pais { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DTOUbicacion ComoDTO() 
        {
            return new DTOUbicacion 
            {
                Id = this.Id,
                CodigoPostal = this.CodigoPostal,
                NumeroExterior = this.NumeroExterior,
                NumeroInterior = this.NumeroInterior,
                Calle = this.Calle,
                Colonia = this.Colonia,
                Ciudad = this.Ciudad,
                Estado = this.Estado,
                Pais = this.Pais.ComoDTO(),
                FechaCreacion = this.FechaCreacion.ToString("o"),
            };
        }

        public void Actualizar(DTOUbicacion modificaciones, Pais pais) 
        {
            CodigoPostal = modificaciones.CodigoPostal;
            NumeroExterior = modificaciones.NumeroExterior;
            NumeroInterior = modificaciones.NumeroInterior;
            Calle = modificaciones.Calle;
            Colonia = modificaciones.Colonia;
            Ciudad = modificaciones.Ciudad;
            Estado = modificaciones.Estado;
            Pais = pais;
        }
    }
}