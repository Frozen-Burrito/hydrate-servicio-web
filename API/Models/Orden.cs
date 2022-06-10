using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ServicioHydrate.Modelos
{
    public class Orden
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }

        // El estado actual de la orden.
        public EstadoOrden Estado { get; set; }

        // El usuario que realizó la orden.
        public Usuario Cliente { get; set; }

        // La lista de productos que el cliente compró en esta orden.
        public virtual ICollection<ProductosOrdenados> Productos { get; set; }
    }
}
