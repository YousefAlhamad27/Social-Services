using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class zzz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             

            migrationBuilder.RenameTable(
                name: "Notifcations",
                newName: "Notifications");
 
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserID",
                table: "Notifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notifications",
                table: "Notifications");

            migrationBuilder.RenameTable(
                name: "Notifications",
                newName: "Notifcations");

            migrationBuilder.RenameIndex(
                name: "IX_Notifications_UserID",
                table: "Notifcations",
                newName: "IX_Notifcations_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notifcations",
                table: "Notifcations",
                column: "NotificationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifcations_Users_UserID",
                table: "Notifcations",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
