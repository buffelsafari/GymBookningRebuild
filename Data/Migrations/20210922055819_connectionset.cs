using Microsoft.EntityFrameworkCore.Migrations;

namespace GymBooking.Data.Migrations
{
    public partial class connectionset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_GymClasses_GymClassId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass");

            migrationBuilder.RenameTable(
                name: "ApplicationUserGymClass",
                newName: "ApplicationUsersGymClasses");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserGymClass_GymClassId",
                table: "ApplicationUsersGymClasses",
                newName: "IX_ApplicationUsersGymClasses_GymClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsersGymClasses",
                table: "ApplicationUsersGymClasses",
                columns: new[] { "ApplicationUserId", "GymClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersGymClasses_AspNetUsers_ApplicationUserId",
                table: "ApplicationUsersGymClasses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsersGymClasses_GymClasses_GymClassId",
                table: "ApplicationUsersGymClasses",
                column: "GymClassId",
                principalTable: "GymClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersGymClasses_AspNetUsers_ApplicationUserId",
                table: "ApplicationUsersGymClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsersGymClasses_GymClasses_GymClassId",
                table: "ApplicationUsersGymClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsersGymClasses",
                table: "ApplicationUsersGymClasses");

            migrationBuilder.RenameTable(
                name: "ApplicationUsersGymClasses",
                newName: "ApplicationUserGymClass");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsersGymClasses_GymClassId",
                table: "ApplicationUserGymClass",
                newName: "IX_ApplicationUserGymClass_GymClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass",
                columns: new[] { "ApplicationUserId", "GymClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_GymClasses_GymClassId",
                table: "ApplicationUserGymClass",
                column: "GymClassId",
                principalTable: "GymClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
