using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class PaymentNullableSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_TenantSubscriptions_TenantSubscriptionId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "TenantSubscriptionId",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_TenantSubscriptions_TenantSubscriptionId",
                table: "Payments",
                column: "TenantSubscriptionId",
                principalTable: "TenantSubscriptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_TenantSubscriptions_TenantSubscriptionId",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "TenantSubscriptionId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_TenantSubscriptions_TenantSubscriptionId",
                table: "Payments",
                column: "TenantSubscriptionId",
                principalTable: "TenantSubscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
