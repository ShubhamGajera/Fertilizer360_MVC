using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fertilizer360.Migrations
{
    /// <inheritdoc />
    public partial class AddFertilizerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Shops",
                table: "Shops");

            migrationBuilder.RenameTable(
                name: "Shops",
                newName: "shops");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "shops",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "shops",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "shops",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "shops",
                newName: "district");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "shops",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "shops",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "shops",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WorkTime",
                table: "shops",
                newName: "work_time");

            migrationBuilder.RenameColumn(
                name: "VillageOrTaluka",
                table: "shops",
                newName: "village_or_taluka");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "shops",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "shops",
                newName: "created_at");

            migrationBuilder.AlterColumn<string>(
                name: "state",
                table: "shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "district",
                table: "shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "village_or_taluka",
                table: "shops",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_shops",
                table: "shops",
                column: "id");

            migrationBuilder.CreateTable(
                name: "fertilizer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stocks = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fertilizer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fertilizer_shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_fertilizer_ShopId",
                table: "fertilizer",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fertilizer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shops",
                table: "shops");

            migrationBuilder.RenameTable(
                name: "shops",
                newName: "Shops");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "Shops",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Shops",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Shops",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "district",
                table: "Shops",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Shops",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Shops",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Shops",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "work_time",
                table: "Shops",
                newName: "WorkTime");

            migrationBuilder.RenameColumn(
                name: "village_or_taluka",
                table: "Shops",
                newName: "VillageOrTaluka");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Shops",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Shops",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Shops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Shops",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "District",
                table: "Shops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VillageOrTaluka",
                table: "Shops",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shops",
                table: "Shops",
                column: "Id");
        }
    }
}
