using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ServicioHydrate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(config => 
                    {
                        // Obtener el string de conexión a Azure App Configuration.
                        IConfiguration settings = config.Build();
                        string connectionString = settings.GetConnectionString("AppConfig");

                        // Cargar la configuracion desde Azure App Configuration.
                        config.AddAzureAppConfiguration(connectionString);
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
