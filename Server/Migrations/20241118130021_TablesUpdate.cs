using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class TablesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attempts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "options",
                table: "Questions");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "Questions",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "correctAnswer",
                table: "Questions",
                newName: "CorrectAnswer");

            migrationBuilder.RenameColumn(
                name: "userAnswer",
                table: "Questions",
                newName: "OptionsJson");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Questions",
                newName: "QuestionId");

            migrationBuilder.AlterColumn<int>(
                name: "ParagraphId",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "ParagraphText",
                table: "Paragraphs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParagraphWordCount",
                table: "Paragraphs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "QuestionId");

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SeesionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParagraphId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SeesionId);
                });

            migrationBuilder.CreateTable(
                name: "ReadingTimes",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    ReadingTimeValue = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingTimes", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_ReadingTimes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SeesionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Wpm = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Results_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SeesionId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadingTimes");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ParagraphWordCount",
                table: "Paragraphs");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Questions",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "CorrectAnswer",
                table: "Questions",
                newName: "correctAnswer");

            migrationBuilder.RenameColumn(
                name: "OptionsJson",
                table: "Questions",
                newName: "userAnswer");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Questions",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "ParagraphId",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<List<string>>(
                name: "options",
                table: "Questions",
                type: "text[]",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "ParagraphText",
                table: "Paragraphs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "ParagraphId");

            migrationBuilder.CreateTable(
                name: "Attempts",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "text", nullable: false),
                    ParagraphId = table.Column<int>(type: "integer", nullable: false),
                    ReadingTime = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false),
                    Wpm = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attempts", x => x.UserName);
                });
        }
    }
}
