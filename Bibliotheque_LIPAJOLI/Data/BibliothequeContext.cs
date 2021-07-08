using Bibliotheque_LIPAJOLI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bibliotheque_LIPAJOLI.Data
{
    public class BibliothequeContext : DbContext
    {
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Usager> Usagers { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }

        public BibliothequeContext(DbContextOptions<BibliothequeContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Livre>().ToTable("Livre");
            modelBuilder.Entity<Usager>().ToTable("Usager");

            modelBuilder.Entity<Emprunt>().ToTable("Emprunt")
                .HasKey(emprunt => new { emprunt.CodeLivre, emprunt.NumAbonne });

        }
    }
}
