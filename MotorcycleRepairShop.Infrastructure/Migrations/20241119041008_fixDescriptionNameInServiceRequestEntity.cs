using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleRepairShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDescriptionNameInServiceRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueDescripton",
                table: "ServiceRequest");

            migrationBuilder.AddColumn<string>(
                name: "IssueDescription",
                table: "ServiceRequest",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueDescription",
                table: "ServiceRequest");

            migrationBuilder.AddColumn<string>(
                name: "IssueDescripton",
                table: "ServiceRequest",
                type: "longtext",
                nullable: false);
        }
    }
}
