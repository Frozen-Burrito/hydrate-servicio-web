using System;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTOProducto
    {
        public int Id { get; set; }

        [MaxLength(32)]
        public string Nombre { get; set; }

        [Range(0.0, 100000.0)]
        public decimal PrecioUnitario { get; set; }

        [MaxLength(300)]
        public string Descripcion { get; set; }

        [Range(0, 100)]
        public int CantidadOrdenada { get; set; }

        [Range(0, 1000)]
        public int Disponibles { get; set; }

        [MaxLength(128)]
        public string UrlImagen { get; set; }
    }
}