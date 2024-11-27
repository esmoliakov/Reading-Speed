﻿// <auto-generated />
using System;
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
    [Migration("20241118130021_TablesUpdate")]
    partial class TablesUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.Models.ParagraphEntity", b =>
                {
                    b.Property<int>("ParagraphId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ParagraphId"));

                    b.Property<string>("ParagraphText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ParagraphWordCount")
                        .HasColumnType("integer");

                    b.HasKey("ParagraphId");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("Shared.Models.QuestionEntity", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("QuestionId"));

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OptionsJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ParagraphId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("QuestionId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Shared.Models.ReadingTimeEntity", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("ReadingTimeValue")
                        .HasColumnType("interval");

                    b.HasKey("SessionId");

                    b.ToTable("ReadingTimes");
                });

            modelBuilder.Entity("Shared.Models.ResultEntity", b =>
                {
                    b.Property<int>("SessionId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Wpm")
                        .HasColumnType("integer");

                    b.HasKey("SessionId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("Shared.Models.SessionEntity", b =>
                {
                    b.Property<int>("SeesionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SeesionId"));

                    b.Property<int>("ParagraphId")
                        .HasColumnType("integer");

                    b.HasKey("SeesionId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Shared.Models.ReadingTimeEntity", b =>
                {
                    b.HasOne("Shared.Models.SessionEntity", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("Shared.Models.ResultEntity", b =>
                {
                    b.HasOne("Shared.Models.SessionEntity", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });
#pragma warning restore 612, 618
        }
    }
}