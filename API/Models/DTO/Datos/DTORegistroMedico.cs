using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTORegistroMedico
    {
        public int Id { get; set; }

        public int IdPerfil { get; set; }

        public float Hipervolemia { get; set; }

        public float PesoPostDialisis { get; set; }

        public float AguaExtracelular { get; set; }

        public float Normovolemia { get; set; }

        public float GananciaRegistrada { get; set; }

        public float GananciaReal { get; set; }

        [MaxLength(33)]
        public string FechaProxCita { get; set; }

        [MaxLength(33)]
        public string FechaDeCreacion { get; set; }

        public DatosMedicos ComoNuevoModelo()
        {
            DateTime fecha;

            bool strISO8601Valido = DateTime
                .TryParse(this.FechaProxCita, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            if (!strISO8601Valido)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            DateTime fechaDeCreacion;

            bool fechaDeCreacionEsValida = DateTime
                .TryParse(this.FechaDeCreacion, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaDeCreacion);

            if (!fechaDeCreacionEsValida)
            {
                throw new FormatException("Se esperaba un string con formato ISO 8601, pero el string recibido no es válido");  
            }

            return new DatosMedicos()
            {
                IdPerfil = this.IdPerfil,
                Hipervolemia = this.Hipervolemia,
                PesoPostDialisis = this.PesoPostDialisis,
                AguaExtracelular = this.AguaExtracelular,
                Normovolemia = this.Normovolemia,
                GananciaRegistrada = this.GananciaRegistrada,
                GananciaReal = this.GananciaReal,
                FechaProxCita = fecha,
                FechaCreacion = fechaDeCreacion,
            };
        }
    }
}