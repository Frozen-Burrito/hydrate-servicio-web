using System;

namespace ServicioHydrate.Modelos.Enums 
{
    [Flags]
    public enum DiasDeLaSemana 
    {
        LUNES           = 0b_0000_0001,
        MARTES          = 0b_0000_0010,
        MIERCOLES       = 0b_0000_0100,
        JUEVES          = 0b_0000_1000,
        VIERNES         = 0b_0001_0000,
        SABADO          = 0b_0010_0000,
        DOMINGO         = 0b_0100_0000,
        TODOS_LOS_DIAS  = 0b_1111_1111,
    }
}