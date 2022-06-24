using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTOOrden
    {
        public Guid Id { get; set; }

        [MaxLength(33)]
        [DataType("char")]
        public string Fecha { get; set; }

        // El estado actual de la orden.
        public EstadoOrden Estado { get; set; }

        // El total, obtenido con la suma de todos los precios de los productos.
        [Range(0.0, 1000000.0)]
        public decimal MontoTotal { get; set; }

        // El usuario que realizó la orden.
        public Guid IdCliente { get; set; }

        // La lista de productos que el cliente compró en esta orden.
        public List<DTOProductoCantidad> Productos { get; set; }

        public long MontoTotalEnCentavos 
        { 
            get { return (long) (MontoTotal * 100); }
        }
    }
}