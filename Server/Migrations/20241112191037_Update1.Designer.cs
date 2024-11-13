﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Database;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ReadingSpeedDbContext))]
    [Migration("20241112191037_Update1")]
    partial class Update1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.Models.Attempt", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<int>("ParagraphId")
                        .HasColumnType("integer");

                    b.Property<int>("ReadingTime")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("Wpm")
                        .HasColumnType("integer");

                    b.HasKey("UserName");

                    b.ToTable("Attempts");
                });

            modelBuilder.Entity("Shared.Models.Paragraph", b =>
                {
                    b.Property<int>("ParagraphId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ParagraphId"));

                    b.Property<string>("ParagraphText")
                        .HasColumnType("text");

                    b.HasKey("ParagraphId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("Shared.Models.Question", b =>
                {
                    b.Property<int>("ParagraphId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ParagraphId"));

                    b.Property<string>("correctAnswer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("id")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<List<string>>("options")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userAnswer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ParagraphId");

                    b.ToTable("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
