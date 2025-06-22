using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Exam");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subjects",
                newSchema: "Exam");

            migrationBuilder.RenameTable(
                name: "QuestionsBank",
                newName: "QuestionsBank",
                newSchema: "Exam");

            migrationBuilder.RenameTable(
                name: "ExamConfigs",
                newName: "ExamConfigs",
                newSchema: "Exam");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Subjects",
                schema: "Exam",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "QuestionsBank",
                schema: "Exam",
                newName: "QuestionsBank");

            migrationBuilder.RenameTable(
                name: "ExamConfigs",
                schema: "Exam",
                newName: "ExamConfigs");
        }
    }
}
