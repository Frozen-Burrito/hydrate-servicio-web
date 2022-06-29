using System;
using System.ComponentModel.DataAnnotations.Schema;

using ServicioHydrate.Modelos.DTO.Datos;
using ServicioHydrate.Modelos.Enums;

namespace ServicioHydrate.Modelos.Datos 
{
    //TODO: revisar la estructura de este modelo, por ahora no es la mejor.
    public class Rutina 
    {
        public int Id { get; set; }

		[Column("id_perfil")]
        public int IdPerfil { get; set; }
        public Perfil PerfilDeUsuario { get; set; }

        public DiasDeLaSemana Dias { get; set; }

        public TimeOnly Hora { get; set; }

		[Column("id_actividad")]
        public int IdActividad { get; set; }
        public ActividadFisica RegistroDeActividad { get; set; }
    }
}