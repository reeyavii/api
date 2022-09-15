using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class updatefk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StallID",
                table: "UserInformations",
                newName: "StallId");

            migrationBuilder.AlterColumn<int>(
                name: "StallId",
                table: "UserInformations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations",
                column: "StallId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Stalls_StallId",
                table: "UserInformations",
                column: "StallId",
                principalTable: "Stalls",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Stalls_StallId",
                table: "UserInformations");

            migrationBuilder.DropIndex(
                name: "IX_UserInformations_StallId",
                table: "UserInformations");

            migrationBuilder.RenameColumn(
                name: "StallId",
                table: "UserInformations",
                newName: "StallID");

            migrationBuilder.AlterColumn<int>(
                name: "StallID",
                table: "UserInformations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
