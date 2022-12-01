using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Migrations
{
    public partial class farmstatisticsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FarmStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastFeedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalFeedingsCount = table.Column<int>(type: "int", nullable: false),
                    AverageFeedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastDrinkTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalDrinkingsCount = table.Column<int>(type: "int", nullable: false),
                    AverageDrinkTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FarmId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FarmStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FarmStatistics_Farms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FarmStatistics_FarmId",
                table: "FarmStatistics",
                column: "FarmId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FarmStatistics");
        }
    }
}
