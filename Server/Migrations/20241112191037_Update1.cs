using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_QuizQuestions_QuizQuestionsParagraphId",
                table: "Question");

            migrationBuilder.DropTable(
                name: "QuizQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuizQuestionsParagraphId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuizQuestionsParagraphId",
                table: "Question");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Questions",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ParagraphId",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "ParagraphId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "ParagraphId",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "Question",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "QuizQuestionsParagraphId",
                table: "Question",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "id");

            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    ParagraphId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.ParagraphId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuizQuestionsParagraphId",
                table: "Question",
                column: "QuizQuestionsParagraphId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_QuizQuestions_QuizQuestionsParagraphId",
                table: "Question",
                column: "QuizQuestionsParagraphId",
                principalTable: "QuizQuestions",
                principalColumn: "ParagraphId");
        }
    }
}
