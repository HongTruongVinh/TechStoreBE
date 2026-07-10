using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechStore.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionCode",
                table: "Payments",
                newName: "PaymentCode");

            migrationBuilder.AddColumn<string>(
                name: "BankReferenceCode",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankReferenceCode",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "TransactionCode",
                table: "Payments",
                newName: "PaymentCode");
        }
    }
}
