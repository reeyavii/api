using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations",
                column: "StallId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations",
                column: "StallId",
                unique: true);
        }
    }
}
