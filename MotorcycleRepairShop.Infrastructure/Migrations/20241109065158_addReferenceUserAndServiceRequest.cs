using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleRepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addReferenceUserAndServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ServiceRequest",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequest_CustomerId",
                table: "ServiceRequest",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequest_ApplicationUser_CustomerId",
                table: "ServiceRequest",
                column: "CustomerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequest_ApplicationUser_CustomerId",
                table: "ServiceRequest");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequest_CustomerId",
                table: "ServiceRequest");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ServiceRequest");
        }
    }
}
