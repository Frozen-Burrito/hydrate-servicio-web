using System.ComponentModel.DataAnnotations;

using ServicioHydrate.Modelos.DTO.Datos; 

namespace ServicioHydrate.Modelos.Datos
{
    public class DatosDeActividad 
    {
        public int Id { get; set; } 

        [Range(0.0, 25.0)]
        public double METs { get; set; }

        [Range(0.0, 50.0)]
        public double VelocidadPromedioKMH { get; set; }

        public DTODatosActividad ComoDTO()
        {
            return new DTODatosActividad
            {
                Id = this.Id,
                METs = this.METs,
                VelocidadPromedioKMH = this.VelocidadPromedioKMH,
            };
        }
    }
}
