using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_a_chatbot_on_off_switch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: new Guid("39e69394-2ad0-484d-b946-34662ea1e946"));

            migrationBuilder.AddColumn<bool>(
                name: "isAiChatbotEnabled",
                table: "SystemConfigs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "Id", "IsShowImportantNotification", "isAiChatbotEnabled" },
                values: new object[] { new Guid("c37aa5ee-00e5-426d-9748-027787cc1a79"), false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SystemConfigs",
                keyColumn: "Id",
                keyValue: new Guid("c37aa5ee-00e5-426d-9748-027787cc1a79"));

            migrationBuilder.DropColumn(
                name: "isAiChatbotEnabled",
                table: "SystemConfigs");

            migrationBuilder.InsertData(
                table: "SystemConfigs",
                columns: new[] { "Id", "IsShowImportantNotification" },
                values: new object[] { new Guid("39e69394-2ad0-484d-b946-34662ea1e946"), true });
        }
    }
}
