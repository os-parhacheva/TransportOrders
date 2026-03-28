using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportOrders.Infrasrtructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitySender = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressSender = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityRecipient = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AddressRecipient = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CargoWeight = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
