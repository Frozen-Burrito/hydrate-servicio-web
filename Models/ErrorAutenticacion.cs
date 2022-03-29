using System;

namespace ServicioHydrate.Autenticacion
{
    // Contiene todos los diferentes tipos de errores de autenticación posibles.
    public enum ErrorAutenticacion 
    {
        NINGUNO, // No hubo ningún error en la autenticación.
        ERROR_CREDENCIALES, // Existe un error con las credenciales del usuario.
        USUARIO_EXISTE, // Ya existe previamente un usuario registrado (durante el registro).
        USUARIO_NO_EXISTE, // No existe un usuario registrado (durante el inicio de sesión).
        PASSWORD_INCORRECTO, // Las contraseñas no coinciden.
        FORMATO_INCORRECTO, // Una petición de autenticación sin un username ni un email.
        SERVICIO_NO_DISPONIBLE, // Error del servicio.
    }

    /// Un error de autenticación, que puede ser enviado a Logs o como respuesta.
    public class MensajeErrorAutenticacion
    {
        public ErrorAutenticacion Tipo { get; set; }
        public string Mensaje { get; set; }
    }
}