
using ServicioHydrate.Modelos.Datos;

namespace ServicioHydrate.Modelos.DTO.Datos
{
    public class DTODatosActividad 
    {
        public int Id { get; set; }

        public double METs { get; set; }

        public double VelocidadPromedioKMH { get; set; }

        public DatosDeActividad ComoNuevoModelo()
        {
            return new DatosDeActividad 
            {
                METs = this.METs,
                VelocidadPromedioKMH = this.VelocidadPromedioKMH,
            };
        }
    }
}