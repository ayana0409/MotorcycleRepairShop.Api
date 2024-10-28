using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleRepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceRequestPartEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceRequestPart",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<int>(type: "int", nullable: false),
                    WarrantyTo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestPart", x => new { x.PartId, x.ServiceRequestId });
                    table.ForeignKey(
                        name: "FK_ServiceRequestPart_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceRequestPart_ServiceRequest_ServiceRequestId",
                        column: x => x.ServiceRequestId,
                        principalTable: "ServiceRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestPart_ServiceRequestId",
                table: "ServiceRequestPart",
                column: "ServiceRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequestPart");
        }
    }
}
