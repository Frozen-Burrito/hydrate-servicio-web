using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            if (_env.IsProduction()) 
            {
                services.AddDbContext<ContextoDB>(options => 
                    options.UseSqlServer(Configuration.GetConnectionString("DbPrincipal")));
            } else 
            {
                services.AddDbContext<ContextoDB>(options => 
                    options.UseSqlite("Data Source=db_desarrollo.db"));
            }

            services.AddCors();

            services.Configure<AppConfig>(Configuration.GetSection("AppConfig"));

            services.AddScoped<GeneradorDeToken>();

            services.AddScoped<IServicioUsuarios, RepositorioUsuarios>();
            services.AddScoped<IServicioRecursos, RepositorioRecursos>();

            services.AddControllers();

            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "Hydrate - Servicio Web",
                        Version = "v1",
                        Description = "API web para el proyecto Hydrate."
                    }
                );
            });

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();

                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Hydrate v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseMiddleware<MiddlewareJWT>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
