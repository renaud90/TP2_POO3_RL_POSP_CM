//using Bibliotheques.ApplicationCore.Entites;
//using Microsoft.AspNetCore.HttpsPolicy;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;
using Bibliotheques.ApplicationCore.Interfaces;
using Bibliotheques.ApplicationCore.Services;
using Bibliotheques.Infrastructure.Repositories;
using Bibliotheques.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;


namespace Bibliotheques.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BibliothequeContext>(options =>
                options
                    .UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                    
            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
            services.AddScoped<IBibliothequeService, BibliothequeService>();

            services.AddControllers()
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    
                    Title = "API de gestion des emprunts de la bibliothèque Lipajoli", 
                    Version = "v1",
                    Description = "Système permettant de créer, modifier, supprimer et consultater des emprunts de livres",
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("http://www.apache.org")
                    },
                    Contact = new OpenApiContact
                    {
                        Name = "Pierre-Renaud Rousseau",
                        Email = "prr@lipajoli.com",
                        Url = new Uri("https://bibliolipajoli.com/")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bibliotheques.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
