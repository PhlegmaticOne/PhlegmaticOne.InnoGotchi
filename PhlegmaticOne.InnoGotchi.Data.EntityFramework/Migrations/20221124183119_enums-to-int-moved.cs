using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Migrations
{
    public partial class enumstointmoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ThirstyLevel",
                table: "InnoGotchies",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Normal");

            migrationBuilder.AlterColumn<int>(
                name: "HungerLevel",
                table: "InnoGotchies",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Normal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ThirstyLevel",
                table: "InnoGotchies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Normal",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "HungerLevel",
                table: "InnoGotchies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Normal",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }
    }
}
