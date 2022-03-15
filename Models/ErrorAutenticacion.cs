using System;

namespace ServicioHydrate.Autenticacion
{
    public enum ErrorAutenticacion 
    {
        NINGUNO,
        ERROR_CREDENCIALES,
        USUARIO_EXISTE,
        USUARIO_NO_EXISTE,
        PASSWORD_INCORRECTO,
        FORMATO_INCORRECTO,
        SERVICIO_NO_DISPONIBLE,
    }

    public class MensajeErrorAutenticacion
    {
        public ErrorAutenticacion Tipo { get; set; }
        public string Mensaje { get; set; }
    }
}