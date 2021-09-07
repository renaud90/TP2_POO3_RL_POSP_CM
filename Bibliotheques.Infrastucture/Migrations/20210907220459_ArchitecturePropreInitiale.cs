using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bibliotheques.Infrastucture.Migrations
{
    public partial class ArchitecturePropreInitiale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livre",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CodeLivre = table.Column<string>(type: "TEXT", nullable: true),
                    Isbn10 = table.Column<string>(type: "TEXT", nullable: false),
                    Isbn13 = table.Column<string>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Categorie = table.Column<string>(type: "TEXT", nullable: false),
                    Quantite = table.Column<int>(type: "INTEGER", nullable: false),
                    Prix = table.Column<double>(type: "REAL", nullable: false),
                    Auteurs = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumAbonne = table.Column<string>(type: "TEXT", nullable: true),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Defaillance = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usager", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emprunt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LivreId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsagerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateEmprunt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateRetour = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emprunt_Livre_LivreId",
                        column: x => x.LivreId,
                        principalTable: "Livre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emprunt_Usager_UsagerId",
                        column: x => x.UsagerId,
                        principalTable: "Usager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emprunt_LivreId_UsagerId",
                table: "Emprunt",
                columns: new[] { "LivreId", "UsagerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Emprunt_UsagerId",
                table: "Emprunt",
                column: "UsagerId");
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
