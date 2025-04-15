using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fertilizer360.Migrations
{
    /// <inheritdoc />
    public partial class CreateOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_AspNetUsers_user_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "FK_orders_fertilizer_fertilizer_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "IX_orders_user_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "price",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "subtotal",
                table: "orders",
                newName: "Subtotal");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "orders",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "orders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "fertilizer_id",
                table: "orders",
                newName: "FertilizerId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "orders",
                newName: "OrderDate");

            migrationBuilder.RenameIndex(
                name: "IX_orders_fertilizer_id",
                table: "orders",
                newName: "IX_orders_FertilizerId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Subtotal",
                table: "orders",
                type: "decimal(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_fertilizer_FertilizerId",
                table: "orders",
                column: "FertilizerId",
                principalTable: "fertilizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_fertilizer_FertilizerId",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "orders");

            migrationBuilder.RenameColumn(
                name: "Subtotal",
                table: "orders",
                newName: "subtotal");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "orders",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "orders",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "FertilizerId",
                table: "orders",
                newName: "fertilizer_id");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "orders",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_orders_FertilizerId",
                table: "orders",
                newName: "IX_orders_fertilizer_id");

            migrationBuilder.AlterColumn<decimal>(
                name: "subtotal",
                table: "orders",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "user_id",
                table: "orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_AspNetUsers_user_id",
                table: "orders",
                column: "user_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_fertilizer_fertilizer_id",
                table: "orders",
                column: "fertilizer_id",
                principalTable: "fertilizer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
