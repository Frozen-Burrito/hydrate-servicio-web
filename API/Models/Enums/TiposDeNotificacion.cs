using System;

namespace ServicioHydrate.Modelos.Enums
{
    [Flags]
    public enum TiposDeNotificacion
    {
        NOTIFIACIONES_DESACTIVADAS  = 0b_0000_0000, 
        RECORDATORIOS_METAS         = 0b_0000_0001,
        ALERTAS_BATERIA_DISPOSITIVO = 0b_0000_0010,
        RECORDATORIOS_RUTINA        = 0b_0000_0100,
        RECORDATORIOS_DESCANSO      = 0b_0000_1000,
        TODAS                       = 0b_1111_1111,
    }
}