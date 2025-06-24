using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubmissionService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAnswerSchema_SubmissionService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NarrativeAnswerText",
                schema: "Submission",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "SelectedOption",
                schema: "Submission",
                table: "Answers",
                newName: "AnswerValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnswerValue",
                schema: "Submission",
                table: "Answers",
                newName: "SelectedOption");

            migrationBuilder.AddColumn<string>(
                name: "NarrativeAnswerText",
                schema: "Submission",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
