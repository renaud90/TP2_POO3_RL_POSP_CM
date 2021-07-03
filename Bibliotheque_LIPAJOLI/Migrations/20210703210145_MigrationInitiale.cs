using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotheque_LIPAJOLI.Migrations
{
    public partial class MigrationInitiale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livre",
                columns: table => new
                {
                    CodeLivre = table.Column<string>(type: "TEXT", nullable: false),
                    Isbn10 = table.Column<string>(type: "TEXT", nullable: true),
                    Isbn13 = table.Column<string>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Categorie = table.Column<string>(type: "TEXT", nullable: false),
                    Quantite = table.Column<int>(type: "INTEGER", nullable: false),
                    Prix = table.Column<double>(type: "REAL", nullable: false),
                    Auteurs = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livre", x => x.CodeLivre);
                });

            migrationBuilder.CreateTable(
                name: "Usager",
                columns: table => new
                {
                    NumAbonne = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Defaillance = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usager", x => x.NumAbonne);
                });

            migrationBuilder.CreateTable(
                name: "Emprunt",
                columns: table => new
                {
                    CodeLivre = table.Column<string>(type: "TEXT", nullable: false),
                    NumAbonne = table.Column<string>(type: "TEXT", nullable: false),
                    DateEmprunt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunt", x => new { x.CodeLivre, x.NumAbonne });
                    table.ForeignKey(
                        name: "FK_Emprunt_Livre_CodeLivre",
                        column: x => x.CodeLivre,
                        principalTable: "Livre",
                        principalColumn: "CodeLivre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emprunt_Usager_NumAbonne",
                        column: x => x.NumAbonne,
                        principalTable: "Usager",
                        principalColumn: "NumAbonne",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprunt_NumAbonne",
                table: "Emprunt",
                column: "NumAbonne");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emprunt");

            migrationBuilder.DropTable(
                name: "Livre");

            migrationBuilder.DropTable(
                name: "Usager");
        }
    }
}
