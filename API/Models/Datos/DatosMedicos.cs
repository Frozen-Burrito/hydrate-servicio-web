using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public DateTime FechaCreacion { get; set; }

        public DTORegistroMedico ComoDTO() 
        {
            return new DTORegistroMedico()
            {
                Id = this.Id,
                IdPerfil = this.IdPerfil,
                Hipervolemia = this.Hipervolemia,
                PesoPostDialisis = this.PesoPostDialisis,
                AguaExtracelular = this.AguaExtracelular,
                Normovolemia = this.Normovolemia,
                GananciaRegistrada = this.GananciaRegistrada,
                GananciaReal = this.GananciaReal,
                FechaProxCita = this.FechaProxCita.ToString("o"),
                FechaDeCreacion = this.FechaCreacion.ToString("o"),
            };
        }
    }
}