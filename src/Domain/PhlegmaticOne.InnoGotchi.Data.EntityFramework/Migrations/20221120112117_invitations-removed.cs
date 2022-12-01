using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Migrations
{
    public partial class invitationsremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_UserProfiles_FromProfileId",
                        column: x => x.FromProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_UserProfiles_ToProfileId",
                        column: x => x.ToProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_FromProfileId",
                table: "Invitations",
                column: "FromProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ToProfileId",
                table: "Invitations",
                column: "ToProfileId");
        }
    }
}
