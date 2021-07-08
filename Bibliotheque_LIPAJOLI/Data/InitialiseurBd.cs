﻿using System;
using System.Linq;
using Bibliotheque_LIPAJOLI.Models;
using Microsoft.Extensions.Configuration;

namespace Bibliotheque_LIPAJOLI.Data
{
    public static class InitialiseurBd
    {
        public static void Initialiser(BibliothequeContext contexte, IConfiguration config)
        {
            if (contexte.Livres.Any())
                return;

            var auteurs = config.GetSection("Bibliotheque:Auteurs").Get<string[]>();
            
            // TODO : Ajouter un service qui va gérer la génération de code de livre.

            var livres = new Livre[]
            {
                new Livre
                {
                    Titre = "Python pour les nuls 3e Édition", Isbn10 = "2412053146", Isbn13 = "9782412053140", 
                    Auteurs = ObtenirValeursAuHasardDe(auteurs), Categorie = "Informatique", CodeLivre = "INF001",
                    Prix = 41.95
                },
                new Livre
                {
                    Titre = "Éloquence de la sardine", Isbn10 = "2290238783", Isbn13 = "9782290238783",
                    Auteurs = ObtenirValeursAuHasardDe(auteurs), Categorie = "Sciences", CodeLivre = "SCI001",
                    Prix = 13.95
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
                }
            };

            contexte.Usagers.AddRange(usagers);
            contexte.SaveChanges();

            // TODO : Ajouter des emprunts.
            var emprunts = new Emprunt[]
            {
                new Emprunt()
                {
                    CodeLivre = contexte.Livres
                        .Single(l => l.Titre == "Python pour les nuls 3e Édition")
                        .CodeLivre,
                    NumAbonne = contexte.Usagers
                        .Single(u => u.Prenom == "Martin" && u.Nom == "Caron")
                        .NumAbonne,
                    DateEmprunt = DateTime.Today
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