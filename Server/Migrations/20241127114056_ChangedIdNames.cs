using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIdNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "Questions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ParagraphId",
                table: "Paragraphs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AttemptId",
                table: "Attempts",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Questions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Paragraphs",
                newName: "ParagraphId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Attempts",
                newName: "AttemptId");
        }
    }
}
