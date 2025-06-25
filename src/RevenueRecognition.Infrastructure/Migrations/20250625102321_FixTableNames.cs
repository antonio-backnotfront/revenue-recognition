using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecognition.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractUpdateTypes_Contract_ContractId",
                table: "ContractUpdateTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractUpdateTypes_UpdateTypes_UpdateTypeId",
                table: "ContractUpdateTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountSubscriptions_Discount_DiscountId",
                table: "DiscountSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountSubscriptions_Subscriptions_SubscriptionId",
                table: "DiscountSubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Client_ClientId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_RenewalPeriod_RenewalPeriodId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Software_SoftwareId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UpdateTypes",
                table: "UpdateTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountSubscriptions",
                table: "DiscountSubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractUpdateTypes",
                table: "ContractUpdateTypes");

            migrationBuilder.RenameTable(
                name: "UpdateTypes",
                newName: "UpdateType");

            migrationBuilder.RenameTable(
                name: "Subscriptions",
                newName: "Subscription");

            migrationBuilder.RenameTable(
                name: "DiscountSubscriptions",
                newName: "DiscountSubscription");

            migrationBuilder.RenameTable(
                name: "ContractUpdateTypes",
                newName: "ContractUpdateType");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_SoftwareId",
                table: "Subscription",
                newName: "IX_Subscription_SoftwareId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_RenewalPeriodId",
                table: "Subscription",
                newName: "IX_Subscription_RenewalPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_ClientId",
                table: "Subscription",
                newName: "IX_Subscription_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountSubscriptions_DiscountId",
                table: "DiscountSubscription",
                newName: "IX_DiscountSubscription_DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractUpdateTypes_ContractId",
                table: "ContractUpdateType",
                newName: "IX_ContractUpdateType_ContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UpdateType",
                table: "UpdateType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountSubscription",
                table: "DiscountSubscription",
                columns: new[] { "SubscriptionId", "DiscountId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractUpdateType",
                table: "ContractUpdateType",
                columns: new[] { "UpdateTypeId", "ContractId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUpdateType_Contract_ContractId",
                table: "ContractUpdateType",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUpdateType_UpdateType_UpdateTypeId",
                table: "ContractUpdateType",
                column: "UpdateTypeId",
                principalTable: "UpdateType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountSubscription_Discount_DiscountId",
                table: "DiscountSubscription",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountSubscription_Subscription_SubscriptionId",
                table: "DiscountSubscription",
                column: "SubscriptionId",
                principalTable: "Subscription",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Client_ClientId",
                table: "Subscription",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_RenewalPeriod_RenewalPeriodId",
                table: "Subscription",
                column: "RenewalPeriodId",
                principalTable: "RenewalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Software_SoftwareId",
                table: "Subscription",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractUpdateType_Contract_ContractId",
                table: "ContractUpdateType");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractUpdateType_UpdateType_UpdateTypeId",
                table: "ContractUpdateType");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountSubscription_Discount_DiscountId",
                table: "DiscountSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountSubscription_Subscription_SubscriptionId",
                table: "DiscountSubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Client_ClientId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_RenewalPeriod_RenewalPeriodId",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Software_SoftwareId",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UpdateType",
                table: "UpdateType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscription",
                table: "Subscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountSubscription",
                table: "DiscountSubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContractUpdateType",
                table: "ContractUpdateType");

            migrationBuilder.RenameTable(
                name: "UpdateType",
                newName: "UpdateTypes");

            migrationBuilder.RenameTable(
                name: "Subscription",
                newName: "Subscriptions");

            migrationBuilder.RenameTable(
                name: "DiscountSubscription",
                newName: "DiscountSubscriptions");

            migrationBuilder.RenameTable(
                name: "ContractUpdateType",
                newName: "ContractUpdateTypes");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_SoftwareId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_SoftwareId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_RenewalPeriodId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_RenewalPeriodId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_ClientId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountSubscription_DiscountId",
                table: "DiscountSubscriptions",
                newName: "IX_DiscountSubscriptions_DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_ContractUpdateType_ContractId",
                table: "ContractUpdateTypes",
                newName: "IX_ContractUpdateTypes_ContractId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UpdateTypes",
                table: "UpdateTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscriptions",
                table: "Subscriptions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountSubscriptions",
                table: "DiscountSubscriptions",
                columns: new[] { "SubscriptionId", "DiscountId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContractUpdateTypes",
                table: "ContractUpdateTypes",
                columns: new[] { "UpdateTypeId", "ContractId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUpdateTypes_Contract_ContractId",
                table: "ContractUpdateTypes",
                column: "ContractId",
                principalTable: "Contract",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContractUpdateTypes_UpdateTypes_UpdateTypeId",
                table: "ContractUpdateTypes",
                column: "UpdateTypeId",
                principalTable: "UpdateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountSubscriptions_Discount_DiscountId",
                table: "DiscountSubscriptions",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountSubscriptions_Subscriptions_SubscriptionId",
                table: "DiscountSubscriptions",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Client_ClientId",
                table: "Subscriptions",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_RenewalPeriod_RenewalPeriodId",
                table: "Subscriptions",
                column: "RenewalPeriodId",
                principalTable: "RenewalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Software_SoftwareId",
                table: "Subscriptions",
                column: "SoftwareId",
                principalTable: "Software",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
