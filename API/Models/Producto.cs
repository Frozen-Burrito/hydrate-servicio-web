using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(32)]
        public string Nombre { get; set; }

        [Range(0.00, 100000.00)]
        public decimal PrecioUnitario { get; set; }

        [MaxLength(300)]
        public string Descripcion { get; set; }

        [Range(0, 1000)]
        public int Disponibles { get; set; }

        [MaxLength(128)]
        public string UrlImagen { get; set; }

        public virtual ICollection<ProductosOrdenados> OrdenesDelProducto { get; set; }
    }
}