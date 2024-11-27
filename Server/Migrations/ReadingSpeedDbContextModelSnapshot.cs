﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server.Database;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(ReadingSpeedDbContext))]
    partial class ReadingSpeedDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.Models.AttemptEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ParagraphId")
                        .HasColumnType("integer");

                    b.Property<long>("ReadingTime")
                        .HasColumnType("bigint");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Wpm")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Attempts");
                });

            modelBuilder.Entity("Shared.Models.ParagraphEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ParagraphText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ParagraphWordCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Paragraphs");
                });

            modelBuilder.Entity("Shared.Models.QuestionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

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

                    b.HasKey("Id");

                    b.HasIndex("ParagraphId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Shared.Models.QuestionEntity", b =>
                {
                    b.HasOne("Shared.Models.ParagraphEntity", "Paragraph")
                        .WithMany()
                        .HasForeignKey("ParagraphId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paragraph");
                });
#pragma warning restore 612, 618
        }
    }
}
