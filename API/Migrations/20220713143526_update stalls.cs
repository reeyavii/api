using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    public partial class updatestalls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessType",
                table: "UserInformations");

            migrationBuilder.DropColumn(
                name: "Ownership",
                table: "UserInformations");

            migrationBuilder.RenameColumn(
                name: "PartnerID",
                table: "UserInformations",
                newName: "MiddleInitial");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Stalls",
                newName: "MonthlyPayment");

            migrationBuilder.AlterColumn<int>(
                name: "StallID",
                table: "UserInformations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Age",
                table: "UserInformations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MiddleInitial",
                table: "UserInformations",
                newName: "PartnerID");

            migrationBuilder.RenameColumn(
                name: "MonthlyPayment",
                table: "Stalls",
                newName: "Price");

            migrationBuilder.AlterColumn<string>(
                name: "StallID",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Age",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BusinessType",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ownership",
                table: "UserInformations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
