using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adding_AuditColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "Subjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Exam",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "Subjects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Exam",
                table: "Subjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "Subjects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "Exam",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "QuestionsBank",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Exam",
                table: "QuestionsBank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "QuestionsBank",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Exam",
                table: "QuestionsBank",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "QuestionsBank",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "Exam",
                table: "QuestionsBank",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "ExamConfigs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Exam",
                table: "ExamConfigs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "ExamConfigs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Exam",
                table: "ExamConfigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "ExamConfigs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                schema: "Exam",
                table: "ExamConfigs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Exam",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Exam",
                table: "QuestionsBank");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                schema: "Exam",
                table: "ExamConfigs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Exam",
                table: "ExamConfigs");

            migrationBuilder.DropColumn(
                name: "DeletedAtUtc",
                schema: "Exam",
                table: "ExamConfigs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Exam",
                table: "ExamConfigs");

            migrationBuilder.DropColumn(
                name: "ModifiedAtUtc",
                schema: "Exam",
                table: "ExamConfigs");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                schema: "Exam",
                table: "ExamConfigs");
        }
    }
}
