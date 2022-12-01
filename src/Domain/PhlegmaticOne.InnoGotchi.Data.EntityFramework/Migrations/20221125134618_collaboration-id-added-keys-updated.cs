using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhlegmaticOne.InnoGotchi.Data.EntityFramework.Migrations
{
    public partial class collaborationidaddedkeysupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avatar_UserProfiles_UserProfileId",
                table: "Avatar");

            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_Farms_FarmId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_Farms_UserProfiles_OwnerId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_OwnerId",
                table: "Farms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collaborations",
                table: "Collaborations");

            migrationBuilder.DropIndex(
                name: "IX_Avatar_UserProfileId",
                table: "Avatar");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "Avatar");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AvatarId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FarmId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Collaborations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collaborations",
                table: "Collaborations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AvatarId",
                table: "UserProfiles",
                column: "AvatarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_FarmId",
                table: "UserProfiles",
                column: "FarmId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collaborations_UserProfileId",
                table: "Collaborations",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_Farms_FarmId",
                table: "Collaborations",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Avatar_AvatarId",
                table: "UserProfiles",
                column: "AvatarId",
                principalTable: "Avatar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Farms_FarmId",
                table: "UserProfiles",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Collaborations_Farms_FarmId",
                table: "Collaborations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Avatar_AvatarId",
                table: "UserProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Farms_FarmId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AvatarId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_FarmId",
                table: "UserProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Collaborations",
                table: "Collaborations");

            migrationBuilder.DropIndex(
                name: "IX_Collaborations_UserProfileId",
                table: "Collaborations");

            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "FarmId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Collaborations");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Farms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "Avatar",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Collaborations",
                table: "Collaborations",
                columns: new[] { "UserProfileId", "FarmId" });

            migrationBuilder.CreateIndex(
                name: "IX_Farms_OwnerId",
                table: "Farms",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avatar_UserProfileId",
                table: "Avatar",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Avatar_UserProfiles_UserProfileId",
                table: "Avatar",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Collaborations_Farms_FarmId",
                table: "Collaborations",
                column: "FarmId",
                principalTable: "Farms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_UserProfiles_OwnerId",
                table: "Farms",
                column: "OwnerId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
