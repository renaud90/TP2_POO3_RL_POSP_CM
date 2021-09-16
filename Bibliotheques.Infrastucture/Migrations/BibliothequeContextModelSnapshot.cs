﻿// <auto-generated />
using System;
using Bibliotheques.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bibliotheques.Infrastructure.Migrations
{
    [DbContext(typeof(BibliothequeContext))]
    partial class BibliothequeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Emprunt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateEmprunt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateRetour")
                        .HasColumnType("TEXT");

                    b.Property<int>("LivreId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsagerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UsagerId");

                    b.HasIndex("LivreId", "UsagerId");

                    b.ToTable("Emprunt");
                });

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Livre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Auteurs")
                        .HasColumnType("TEXT");

                    b.Property<string>("Categorie")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CodeLivre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Isbn10")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Isbn13")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Prix")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantite")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Livre");
                });

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Usager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Defaillance")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("NumAbonne")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("Statut")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Usager");
                });

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Emprunt", b =>
                {
                    b.HasOne("Bibliotheques.ApplicationCore.Entites.Livre", "Livre")
                        .WithMany("Emprunts")
                        .HasForeignKey("LivreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bibliotheques.ApplicationCore.Entites.Usager", "Usager")
                        .WithMany("Emprunts")
                        .HasForeignKey("UsagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Livre");

                    b.Navigation("Usager");
                });

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Livre", b =>
                {
                    b.Navigation("Emprunts");
                });

            modelBuilder.Entity("Bibliotheques.ApplicationCore.Entites.Usager", b =>
                {
                    b.Navigation("Emprunts");
                });
#pragma warning restore 612, 618
        }
    }
}
