using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAiConversationContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: new Guid("36fa4d6e-3d48-4b0b-8c11-e61a37bd72b8"));

            migrationBuilder.AddColumn<string>(
                name: "SearchQuery",
                table: "AiConversationContexts",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "Id", "IsShowImportantNotification", "isAiChatbotEnabled" },
                values: new object[] { new Guid("a07d823c-b189-4548-a3bb-870abedbb88e"), true, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: new Guid("a07d823c-b189-4548-a3bb-870abedbb88e"));

            migrationBuilder.DropColumn(
                name: "SearchQuery",
                table: "AiConversationContexts");

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "Id", "IsShowImportantNotification", "isAiChatbotEnabled" },
                values: new object[] { new Guid("36fa4d6e-3d48-4b0b-8c11-e61a37bd72b8"), true, true });
        }
    }
}
