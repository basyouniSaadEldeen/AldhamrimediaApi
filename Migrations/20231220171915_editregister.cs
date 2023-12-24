using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AldhamrimediaApi.Migrations
{
    public partial class editregister : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceFeatures",
                table: "utilities");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ServiceFeatures",
                table: "utilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
