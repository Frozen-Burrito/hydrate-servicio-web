using System;
using System.Collections.Generic;

using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class ResultadoGrafico<T>
    {
        public Eje<T> EjeHorizontal { get; set; }
        public Eje<T> EjeVertical { get; set; }

        public ICollection<TipoDeGrafica> GraficasCompatibles { get; set; }

        public ICollection<DataPoint<T>> Datos { get; set; }

        public static ResultadoGrafico<T> DesdeColeccion(IEnumerable<object> coleccion)
        {
            var resultado = new ResultadoGrafico<T>();
            return resultado;
        }
    }

    public class Eje<T>
    {
        public string Variable { get; set; }

        public T Intervalo { get; set; }

        public Tuple<T, T> Rango { get; set; }
    }

    public class DataPoint<T>
    {
        public T X { get; set; }

        public T Y { get; set; }
    }
}