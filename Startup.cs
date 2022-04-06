using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

using ServicioHydrate.Data;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Utilidades;

namespace ServicioHydrate
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment entorno)
        {
            Configuration = configuration;
            _env = entorno;
        }

        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // Este método es llamado por el runtime. Utiliza este método para agregar servicios al contenedor.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Configurar el contexto de base de datos.
            if (_env.IsProduction()) 
            {
                // En entrono de producción (ya desplegado en Azure) usar SQL Server.
                services.AddDbContext<ContextoDB>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("DbPrincipal")));
            } else 
            {
                // En entrono de desarrollo (local) usar SQLite.
                services.AddDbContext<ContextoDBSqlite>(options => 
                    options.UseSqlite("Data Source=db_desarrollo.db"));
            }

            // Configura Cross-Origin Resource Sharing.
            services.AddCors();

            // Obtiene la configuración del secreto para JWT.
            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            // Incluye el servicio de generación de JWT.
            services.AddScoped<GeneradorDeToken>();

            // Incluye los servicios de acceso a datos.
            services.AddScoped<IServicioUsuarios, RepositorioUsuarios>();
            services.AddScoped<IServicioRecursos, RepositorioRecursos>();

            services.AddScoped<IServicioComentarios, RepositorioComentarios>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetSection("AppConfig:SecretoJWT").Value)
                        ),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            // Agrega los controladores puros (de la API).
            services.AddControllers();

            // Agregar la generación automática de documentación de API con Swagger.
            services.AddSwaggerGen(options => 
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Hydrate - Servicio Web",
                        Version = "v1",
                        Description = "API web para el proyecto Hydrate."
                    }
                );

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Encabezado estándar de autenticación con formato 'Bearer' (\"bearer {token}\").",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            // En producción, los archivos de React serán servidos desde este directorio.
            // "build/" es donde está la versión "compilada" de la app de React.
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // Este método es llamado por el runtime. Usa este método para configurar el pipeline de peticiones HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // Utilizar página de error detallada.
                app.UseDeveloperExceptionPage();

                // Utilizar Swagger y su UI.
                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Hydrate v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // El valor por defecto de Hsts es de 30 días. 
                // Es posible que cambiemos esto para entornos de producción, ver https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Redirige peticiones HTTP a HTTPS.
            app.UseHttpsRedirection();

            // Usa archivos estáticos (css, js, etc.) de la app de React.
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            // Configura el enrutamiento para las url.
            app.UseRouting();

            // Incluir middleware propio para verificar autenticación y autorización
            // con JWT.
            app.UseAuthentication();
            app.UseAuthorization();

            // Utilizar endpoints de los controladores de la API.
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Utilizar Aplicación de Página Única (app de React).
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // Usar React en versión de desarrollo, si el entorno 
                    // de ASP.NET es desarrollo.
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
