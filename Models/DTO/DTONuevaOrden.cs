using System;
using System.Collections.Generic;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTONuevaOrden
    {
        // El Id del usuario que realizó la orden.
        public Guid IdCliente { get; set; }

        // Un diccionario con todos los productos comprados por el cliente en la orden.
        // Cada entrada almacena un <idProducto, cantidad> de un producto.
        public Dictionary<int, int> ProductosComprados { get; set; }
    }
}