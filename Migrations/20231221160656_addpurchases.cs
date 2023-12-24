using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AldhamrimediaApi.Migrations
{
    public partial class addpurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Service_Cost",
                table: "utilities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<int>(
                name: "Required_quantity",
                table: "Purchases",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Service_ImgUrl",
                table: "Purchases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service_Cost",
                table: "utilities");

            migrationBuilder.DropColumn(
                name: "Service_ImgUrl",
                table: "Purchases");

            migrationBuilder.AlterColumn<decimal>(
                name: "Required_quantity",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
