using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewDiscountSeedRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsLoyal",
                table: "Client",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { new DateTime(2029, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loyal Customer Discount", new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5m });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year Sale", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "Id", "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { 3, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<bool>(
                name: "IsLoyal",
                table: "Client",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year Sale", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m });

            migrationBuilder.UpdateData(
                table: "Discount",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "EndDate", "Name", "StartDate", "Value" },
                values: new object[] { new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20m });
        }
    }
}
