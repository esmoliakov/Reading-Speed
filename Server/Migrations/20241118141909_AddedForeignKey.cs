using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Questions_ParagraphId",
                table: "Questions",
                column: "ParagraphId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Paragraphs_ParagraphId",
                table: "Questions",
                column: "ParagraphId",
                principalTable: "Paragraphs",
                principalColumn: "ParagraphId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Paragraphs_ParagraphId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_ParagraphId",
                table: "Questions");
        }
    }
}
