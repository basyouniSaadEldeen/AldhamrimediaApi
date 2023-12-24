using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AldhamrimediaApi.Migrations
{
    public partial class addposter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "utilities",
                newName: "ImageUrlPoster");

            migrationBuilder.RenameColumn(
                name: "ImagePublicId",
                table: "utilities",
                newName: "ImageUrlLogo");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "utilities",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicIdLogo",
                table: "utilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePublicIdPoster",
                table: "utilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicIdLogo",
                table: "utilities");

            migrationBuilder.DropColumn(
                name: "ImagePublicIdPoster",
                table: "utilities");

            migrationBuilder.RenameColumn(
                name: "ImageUrlPoster",
                table: "utilities",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageUrlLogo",
                table: "utilities",
                newName: "ImagePublicId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "utilities",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
