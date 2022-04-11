using System;

namespace ServicioHydrate.Modelos
{
    public class ProductosOrdenados
    {
        public int Cantidad { get; set; }

        public int IdProducto { get; set; }
        public Producto Producto { get; set; }

        public Guid IdOrden { get; set; }
        public Orden Orden { get; set; }
    }
}