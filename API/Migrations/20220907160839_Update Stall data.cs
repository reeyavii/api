using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class UpdateStalldata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stalls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stalls_UserId",
                table: "Stalls",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stalls_AspNetUsers_UserId",
                table: "Stalls",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stalls_AspNetUsers_UserId",
                table: "Stalls");

            migrationBuilder.DropIndex(
                name: "IX_Stalls_UserId",
                table: "Stalls");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stalls");
        }
    }
}
