using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Migrations
{
    public partial class happinessdayscountpropertyadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HappinessDaysCount",
                table: "InnoGotchies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HappinessDaysCount",
                table: "InnoGotchies");
        }
    }
}
