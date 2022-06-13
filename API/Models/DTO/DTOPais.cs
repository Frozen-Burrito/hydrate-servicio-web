
namespace ServicioHydrate.Modelos.DTO 
{
	public class DTOPais 
	{
        public int Id { get; set; }

        public string Codigo { get; set; }

        public Pais ComoNuevoModelo() 
        {
            return new Pais
            {
                Id = this.Id,
                Codigo = this.Codigo
            };
        }
    }
}