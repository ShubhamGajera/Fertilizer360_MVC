using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fertilizer360.Migrations
{
    /// <inheritdoc />
    public partial class FixShopIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fertilizer_shops_ShopId",
                table: "fertilizer");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "fertilizer",
                newName: "shops_id");

            migrationBuilder.RenameIndex(
                name: "IX_fertilizer_ShopId",
                table: "fertilizer",
                newName: "IX_fertilizer_shops_id");

            migrationBuilder.AddForeignKey(
                name: "FK_fertilizer_shops_shops_id",
                table: "fertilizer",
                column: "shops_id",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fertilizer_shops_shops_id",
                table: "fertilizer");

            migrationBuilder.RenameColumn(
                name: "shops_id",
                table: "fertilizer",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_fertilizer_shops_id",
                table: "fertilizer",
                newName: "IX_fertilizer_ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_fertilizer_shops_ShopId",
                table: "fertilizer",
                column: "ShopId",
                principalTable: "shops",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
