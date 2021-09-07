using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Bibliotheques.Infrastucture.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bibliotheques.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hote = CreateHostBuilder(args).Build();

            CreerBdSiExistePas(hote);

            hote.Run();
        }

        private static void CreerBdSiExistePas(IHost hote)
        {
            using var scope = hote.Services.CreateScope();

            var services = scope.ServiceProvider;

            try
            {
                var contexte = services.GetRequiredService<BibliothequeContext>();
                var config = services.GetRequiredService<IConfiguration>();

                InitialiseurBd.Initialiser(contexte, config.GetSection("Bibliotheque:Auteurs").Get<string[]>());
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Une erreur est survenue lors de la création de la base de données");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
