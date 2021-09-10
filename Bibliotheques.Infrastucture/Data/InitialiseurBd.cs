using System;
using System.Linq;
using Bibliotheques.ApplicationCore.Entites;

namespace Bibliotheques.Infrastucture.Data
{
    public static class InitialiseurBd
    {
        public static void Initialiser(BibliothequeContext contexte, string[] auteurs)
        {
            contexte.Database.EnsureCreated();

            if (contexte.Livres.Any())
                return;

            var livres = new Livre[]
            {
                new Livre
                {
                    Titre = "Python pour les nuls 3e Édition", Isbn10 = "2-41205-314-6", Isbn13 = "978-2-41205-314-0",
                    Auteurs = ObtenirValeursAuHasardDe(auteurs), Categorie = "Informatique", CodeLivre = "INF001",
                    Prix = 41.95, Quantite = 1
                },
                new Livre
                {
                    Titre = "Éloquence de la sardine", Isbn10 = "2-29023-878-3", Isbn13 = "978-2-29023-878-3",
                    Auteurs = ObtenirValeursAuHasardDe(auteurs), Categorie = "Sciences", CodeLivre = "SCI001",
                    Prix = 13.95, Quantite = 2
                }
            };

            contexte.Livres.AddRange(livres);
            contexte.SaveChanges();

            var usagers = new Usager[]
            {
                new Usager()
                {
                    Prenom = "Jean-Paul", Nom = "Berger", NumAbonne = "BEJP0001",
                    Email = "jpaul.berger@gmail.com", Statut = Statut.Enseignant
                },
                new Usager()
                {
                    Prenom = "Martin", Nom = "Caron", NumAbonne = "CAMA0002",
                    Email = "martin.caron@gmail.com", Statut = Statut.Etudiant
                },
                new Usager()
                {
                    Prenom = "Eddie", Nom = "Brock", NumAbonne = "BRED0003",
                    Email = "beddie@gmail.com", Statut = Statut.Enseignant
                }
            };

            contexte.Usagers.AddRange(usagers);
            contexte.SaveChanges();

            // TODO : Ajouter des emprunts.
            var emprunts = new Emprunt[]
            {
                new Emprunt()
                {
                    LivreId = contexte.Livres
                        .Single(l => l.Titre == "Python pour les nuls 3e Édition")
                        .Id,
                    UsagerId = contexte.Usagers
                        .Single(u => u.Prenom == "Martin" && u.Nom == "Caron")
                        .Id,
                    DateEmprunt = DateTime.Today
                },
                new Emprunt()
                {
                    LivreId = contexte.Livres
                        .Single(l => l.Titre == "Éloquence de la sardine")
                        .Id,
                    UsagerId = contexte.Usagers
                        .Single(u => u.Prenom == "Eddie" && u.Nom == "Brock")
                        .Id,
                    DateEmprunt = DateTime.Today.AddDays(-8),
                    DateRetour = DateTime.Today.AddDays(-2)
                },
                new Emprunt()
                {
                    LivreId = contexte.Livres
                        .Single(l => l.Titre == "Éloquence de la sardine")
                        .Id,
                    UsagerId = contexte.Usagers
                        .Single(u => u.Prenom == "Jean-Paul" && u.Nom == "Berger")
                        .Id,
                    DateEmprunt = DateTime.Today.AddDays(-8),
                    DateRetour = DateTime.Today
                }, 
                new Emprunt()
                {
                    LivreId = contexte.Livres
                        .Single(l => l.Titre == "Python pour les nuls 3e Édition")
                        .Id,
                    UsagerId = contexte.Usagers
                        .Single(u => u.Prenom == "Eddie" && u.Nom == "Brock")
                        .Id,
                    DateEmprunt = DateTime.Today.AddDays(-20),
                    DateRetour = DateTime.Today.AddDays(-9)
                }
            };

            contexte.Emprunts.AddRange(emprunts);
            contexte.SaveChanges();
        }

        private static string ObtenirValeursAuHasardDe(string[] valeurs)
        {
            if (valeurs == null || valeurs.Length == 0)
                return string.Empty;

            var rnd = new Random();

            var nbValeursAObtenir = rnd.Next(1, valeurs.Length + 1);

            var valeursAObtenir = valeurs.Take(nbValeursAObtenir);

            return string.Join(", ", valeursAObtenir);
        }
    }
}
