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

using Stripe;

using ServicioHydrate.Data;
using ServicioHydrate.Autenticacion;
using ServicioHydrate.Utilidades;
using ServicioHydrate.Formatters;
using Microsoft.Net.Http.Headers;

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
                // String de conexión a la BD principal (es incluido como variable env en
                // el entorno de producción de Azure)
                string strConexion = Configuration.GetConnectionString("DbPrincipal");

                // Usar MySQL en entrono de producción (ya desplegado en Azure).
                services.AddDbContext<ContextoDBMysql>(options => 
                        options.UseMySql(strConexion, ServerVersion.AutoDetect(strConexion)));
                // services.AddDbContext<ContextoDB>(options => 
                //     options.UseSqlServer(Configuration.GetConnectionString("DbPrincipal")));
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

            // Incluye el servicio de filtrado de contenido de comentarios.
            services.AddScoped<IServicioFiltroContenido, FiltroDeComentarios>();

            // Incluye los servicios de acceso a datos.
            services.AddScoped<IServicioUsuarios, RepositorioUsuarios>();
            services.AddScoped<IServicioRecursos, RepositorioRecursos>();

            services.AddScoped<IServicioComentarios, RepositorioComentarios>();

            services.AddScoped<IServicioOrdenes, RepositorioOrdenes>();
            services.AddScoped<IServicioProductos, RepositorioProductos>();

            services.AddScoped<IServicioLlavesDeAPI, RepositorioLlavesDeAPI>();

            // Configurar autenticación usando llaves de API.
            services.AddAuthentication(opciones => 
            {
                opciones.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opciones.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AgregarSoporteParaLlaveDeAPI(opciones => {})
            .AddJwtBearer(opciones => 
            {
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration.GetSection("AppConfig:SecretoJWT").Value)
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddMvc(opciones => 
            {
                opciones.OutputFormatters.Add(new CsvMediaFormatter());
                opciones.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
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

                options.AddSecurityDefinition("Llave de API", new OpenApiSecurityScheme
                {
                    Description = "Encabezado de autenticación por llave de API.",
                    In = ParameterLocation.Header,
                    Name = "x-api-key",
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
            // Configurar llave de API de Stripe
            StripeConfiguration.ApiKey = Configuration.GetSection("AppConfig:StripeApiKey").Value;
            
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

            // Usa archivos estáticos (css, js, etc.) de la app de React.
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

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
