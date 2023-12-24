using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AldhamrimediaApi.Migrations
{
    public partial class addrecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "My_balance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    Type_of_service = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Service_request_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number_of_money_paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Required_quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Account_link = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_userId",
                table: "Purchases",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropColumn(
                name: "My_balance",
                table: "AspNetUsers");
        }
    }
}
