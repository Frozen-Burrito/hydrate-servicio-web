using System;

using ServicioHydrate.Modelos.DTO.Datos;

namespace ServicioHydrate.Modelos.Datos
{
    public class DatosMedicos 
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        public float Hipervolemia { get; set; }

        public float PesoPostDialisis { get; set; }

        public float AguaExtracelular { get; set; }

        public float Normovolemia { get; set; }

        public float GananciaRegistrada { get; set; }

        public float GananciaReal { get; set; }

        public DateTime FechaProxCita { get; set; }
    }
}