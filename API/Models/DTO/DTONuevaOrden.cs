using System;
using System.Collections.Generic;

namespace ServicioHydrate.Modelos.DTO
{
    public class DTONuevaOrden
    {
        // Un diccionario con todos los productos comprados por el cliente en la orden.
        // Cada entrada almacena un <idProducto, cantidad> de un producto.
        public DTOProductoCantidad[] Productos { get; set; }
    }

    public class DTOProductoCantidad 
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}