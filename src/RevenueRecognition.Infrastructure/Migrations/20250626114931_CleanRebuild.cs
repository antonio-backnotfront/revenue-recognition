using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RevenueRecognition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CleanRebuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsLoyal = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RenewalPeriod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RenewalPeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CurrentVersionId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Software_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualClient",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    PESEL = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualClient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualClient_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoftwareVersion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    VersionNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareVersion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwareVersion_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    RenewalPeriodId = table.Column<int>(type: "int", nullable: false),
                    SubscriptionStatusId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_RenewalPeriod_RenewalPeriodId",
                        column: x => x.RenewalPeriodId,
                        principalTable: "RenewalPeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_SubscriptionStatus_SubscriptionStatusId",
                        column: x => x.SubscriptionStatusId,
                        principalTable: "SubscriptionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareVersionId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ContractStatusId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YearsOfSupport = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contract_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_ContractStatus_ContractStatusId",
                        column: x => x.ContractStatusId,
                        principalTable: "ContractStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_SoftwareVersion_SoftwareVersionId",
                        column: x => x.SoftwareVersionId,
                        principalTable: "SoftwareVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountSubscription",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountSubscription", x => new { x.SubscriptionId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_DiscountSubscription_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountSubscription_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPayment_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractPayment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractPayment_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContractUpdateType",
                columns: table => new
                {
                    UpdateTypeId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractUpdateType", x => new { x.UpdateTypeId, x.ContractId });
                    table.ForeignKey(
                        name: "FK_ContractUpdateType_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContractUpdateType_UpdateType_UpdateTypeId",
                        column: x => x.UpdateTypeId,
                        principalTable: "UpdateType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountContract",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    ContractId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountContract", x => new { x.DiscountId, x.ContractId });
                    table.ForeignKey(
                        name: "FK_DiscountContract_Contract_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountContract_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Productivity" },
                    { 2, "Entertainment" }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Address", "Email", "IsLoyal", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main St", "company@example.com", true, "123456789" },
                    { 2, "456 Elm St", "individual@example.com", false, "987654321" }
                });

            migrationBuilder.InsertData(
                table: "ContractStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Paid" },
                    { 2, "Active" },
                    { 3, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "Discount",
                columns: new[] { "Id", "EndDate", "Name", "StartDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year Sale", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10m },
                    { 2, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday", new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 20m }
                });

            migrationBuilder.InsertData(
                table: "RenewalPeriod",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Monthly" },
                    { 2, "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Suspended" }
                });

            migrationBuilder.InsertData(
                table: "UpdateType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bug Fix" },
                    { 2, "Feature" }
                });

            migrationBuilder.InsertData(
                table: "CompanyClient",
                columns: new[] { "Id", "ClientId", "KRS", "Name" },
                values: new object[] { 1, 1, "1234567890", "Example Corp" });

            migrationBuilder.InsertData(
                table: "IndividualClient",
                columns: new[] { "Id", "ClientId", "FirstName", "IsDeleted", "LastName", "PESEL" },
                values: new object[] { 1, 2, "John", false, "Doe", "12345678901" });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "CategoryId", "Cost", "CurrentVersionId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, 99.99m, 1, "Task manager", "SuperProductivity" },
                    { 2, 2, 49.99m, 2, "Casual game", "FunGame" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Login", "Password", "RoleId" },
                values: new object[,]
                {
                    { 1, "admin", "admin123", 1 },
                    { 2, "user", "user123", 2 }
                });

            migrationBuilder.InsertData(
                table: "SoftwareVersion",
                columns: new[] { "Id", "Description", "ReleaseDate", "SoftwareId", "VersionNumber" },
                values: new object[,]
                {
                    { 1, "v1.0 release", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "1.0" },
                    { 2, "v1.0 release", new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "1.0" }
                });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "Id", "ClientId", "Price", "RegisterDate", "RenewalPeriodId", "SoftwareId", "SubscriptionStatusId" },
                values: new object[] { 1, 2, 9.99m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "Contract",
                columns: new[] { "Id", "ClientId", "ContractStatusId", "EndDate", "Paid", "Price", "SoftwareVersionId", "StartDate", "YearsOfSupport" },
                values: new object[] { 1, 1, 2, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, 1000m, 1, new DateTime(2025, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "DiscountSubscription",
                columns: new[] { "DiscountId", "SubscriptionId" },
                values: new object[] { 2, 1 });

            migrationBuilder.InsertData(
                table: "SubscriptionPayment",
                columns: new[] { "Id", "Amount", "PaidAt", "SubscriptionId" },
                values: new object[,]
                {
                    { 1, 9.99m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 9.99m, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, 9.99m, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "ContractPayment",
                columns: new[] { "Id", "Amount", "ContractId", "PaidAt" },
                values: new object[,]
                {
                    { 1, 300m, 1, new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 200m, 1, new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ContractUpdateType",
                columns: new[] { "ContractId", "UpdateTypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "DiscountContract",
                columns: new[] { "ContractId", "DiscountId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClient_ClientId",
                table: "CompanyClient",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ClientId",
                table: "Contract",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ContractStatusId",
                table: "Contract",
                column: "ContractStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_SoftwareVersionId",
                table: "Contract",
                column: "SoftwareVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractPayment_ContractId",
                table: "ContractPayment",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractUpdateType_ContractId",
                table: "ContractUpdateType",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountContract_ContractId",
                table: "DiscountContract",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountSubscription_DiscountId",
                table: "DiscountSubscription",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualClient_ClientId",
                table: "IndividualClient",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Software_CategoryId",
                table: "Software",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareVersion_SoftwareId",
                table: "SoftwareVersion",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_ClientId",
                table: "Subscription",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_RenewalPeriodId",
                table: "Subscription",
                column: "RenewalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SoftwareId",
                table: "Subscription",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriptionStatusId",
                table: "Subscription",
                column: "SubscriptionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPayment_SubscriptionId",
                table: "SubscriptionPayment",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyClient");

            migrationBuilder.DropTable(
                name: "ContractPayment");

            migrationBuilder.DropTable(
                name: "ContractUpdateType");

            migrationBuilder.DropTable(
                name: "DiscountContract");

            migrationBuilder.DropTable(
                name: "DiscountSubscription");

            migrationBuilder.DropTable(
                name: "IndividualClient");

            migrationBuilder.DropTable(
                name: "SubscriptionPayment");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UpdateType");

            migrationBuilder.DropTable(
                name: "Contract");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "ContractStatus");

            migrationBuilder.DropTable(
                name: "SoftwareVersion");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "RenewalPeriod");

            migrationBuilder.DropTable(
                name: "SubscriptionStatus");

            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
