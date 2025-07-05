using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notification");

            migrationBuilder.RenameTable(
                name: "OutboxState",
                newName: "OutboxState",
                newSchema: "Notification");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                newName: "OutboxMessage",
                newSchema: "Notification");

            migrationBuilder.RenameTable(
                name: "NotificationLogs",
                newName: "NotificationLogs",
                newSchema: "Notification");

            migrationBuilder.RenameTable(
                name: "InboxState",
                newName: "InboxState",
                newSchema: "Notification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "OutboxState",
                schema: "Notification",
                newName: "OutboxState");

            migrationBuilder.RenameTable(
                name: "OutboxMessage",
                schema: "Notification",
                newName: "OutboxMessage");

            migrationBuilder.RenameTable(
                name: "NotificationLogs",
                schema: "Notification",
                newName: "NotificationLogs");

            migrationBuilder.RenameTable(
                name: "InboxState",
                schema: "Notification",
                newName: "InboxState");
        }
    }
}
