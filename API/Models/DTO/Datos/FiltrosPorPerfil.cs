
using System;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class FiltrosPorPerfil 
    {
        public int? IdPais { get; set; }

        public SexoUsuario? Sexo { get; set; }

        public Ocupacion? Ocupacion { get; set; }

        public FiltroPorRango<int>? Edad { get; set; }

        public FiltroPorRango<DateTime>? Fecha { get; set; }

        public FiltroPorRango<int>? HorasDiariasDeActFisica { get; set; }
    }

    public class FiltroPorRango<T> 
    {
        public T? Min { get; set; }

        public T? Max { get; set; }
    }
}
#nullable disable