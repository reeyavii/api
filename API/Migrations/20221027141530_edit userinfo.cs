using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class edituserinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInformations_Stalls_StallId",
                table: "UserInformations");

            migrationBuilder.AlterColumn<int>(
                name: "StallId",
                table: "UserInformations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "UserInformations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AlterColumn<int>(
                name: "StallId",
                table: "UserInformations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "UserInformations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInformations_Stalls_StallId",
                table: "UserInformations",
                column: "StallId",
                principalTable: "Stalls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
