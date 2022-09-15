using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class updatestallUserInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stalls_AspNetUsers_UserId",
                table: "Stalls");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_Stalls_UserId",
                table: "Stalls");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Stalls");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations",
                column: "StallId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Stalls",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations",
                column: "StallId");

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
    }
}
