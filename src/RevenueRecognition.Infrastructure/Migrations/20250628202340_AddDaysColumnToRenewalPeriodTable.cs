using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDaysColumnToRenewalPeriodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Days",
                table: "RenewalPeriod",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "RenewalPeriod",
                keyColumn: "Id",
                keyValue: 1,
                column: "Days",
                value: 30);

            migrationBuilder.UpdateData(
                table: "RenewalPeriod",
                keyColumn: "Id",
                keyValue: 2,
                column: "Days",
                value: 365);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                table: "RenewalPeriod");
        }
    }
}
