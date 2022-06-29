using System;
using System.Collections.Generic;

using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOResultadoGrafico<T>
    {
        public Eje<T> EjeHorizontal { get; set; }
        public Eje<T> EjeVertical { get; set; }

        public ICollection<TipoDeGrafica> GraficasCompatibles { get; set; }

        public ICollection<DataPoint<T>> Datos { get; set; }
    }

    public class Eje<T>
    {
        public string NombreVariable { get; set; }

        public T Step { get; set; }

        public Tuple<T, T> Intervalo { get; set; }
    }

    public class DataPoint<T>
    {
        public T X { get; set; }

        public T Y { get; set; }
    }
}