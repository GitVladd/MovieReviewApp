﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieReviewApp.Data;

#nullable disable

namespace MovieService.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240726100742_InitMigration")]
    partial class InitMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryMovie", b =>
                {
                    b.Property<Guid>("CategoriesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MoviesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoriesId", "MoviesId");

                    b.HasIndex("MoviesId");

                    b.ToTable("CategoryMovie");
                });

            modelBuilder.Entity("MovieService.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("a2033ac8-7782-42ad-b4a1-3b6b450677b3"),
                            Name = "Movie"
                        },
                        new
                        {
                            Id = new Guid("e391c4be-d8f0-460a-a5eb-ab66880f700c"),
                            Name = "Series"
                        },
                        new
                        {
                            Id = new Guid("e4357d6d-baf7-459a-8138-f2dc1a79806b"),
                            Name = "Anime"
                        });
                });

            modelBuilder.Entity("MovieService.Models.ContentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ContentTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("427b030e-e5d6-4bd2-bf4f-53d224f58d54"),
                            Name = "Action"
                        },
                        new
                        {
                            Id = new Guid("cf875881-d348-4d85-aeee-aee25d262332"),
                            Name = "Comedy"
                        },
                        new
                        {
                            Id = new Guid("cfd227f1-8017-4af5-bb51-2774c7417506"),
                            Name = "Drama"
                        });
                });

            modelBuilder.Entity("MovieService.Models.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ContentTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("Date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("ContentTypeId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("CategoryMovie", b =>
                {
                    b.HasOne("MovieService.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieService.Models.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieService.Models.Movie", b =>
                {
                    b.HasOne("MovieService.Models.ContentType", "ContentType")
                        .WithMany()
                        .HasForeignKey("ContentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContentType");
                });
#pragma warning restore 612, 618
        }
    }
}
