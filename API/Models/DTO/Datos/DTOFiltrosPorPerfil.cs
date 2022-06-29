
using System;
using ServicioHydrate.Modelos.Enums;

#nullable enable
namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTOFiltrosPorPerfil 
    {
        public Pais? Pais { get; set; }

        public SexoUsuario? Sexo { get; set; }

        public Ocupacion? Ocupacion { get; set; }

        public Tuple<int, int>? Edad { get; set; }

        public Tuple<DateTime, DateTime>? Fecha { get; set; }

        public Tuple<int, int>? HorasDiariasDeActFisica { get; set; }
    }
}
#nullable disable