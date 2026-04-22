using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCreatedAtAndUnitType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Tables",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Suppliers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "StockMovements",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "PurchaseOrders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "PurchaseOrderItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "TnitType",
                table: "Products",
                newName: "UnitType");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Products",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Payments",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Orders",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "OrderItems",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Categories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "CreaedAt",
                table: "Branches",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Tables",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Suppliers",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "StockMovements",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseOrders",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "PurchaseOrderItems",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "UnitType",
                table: "Products",
                newName: "TnitType");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Products",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Payments",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Orders",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "OrderItems",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Categories",
                newName: "CreaedAt");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Branches",
                newName: "CreaedAt");
        }
    }
}
