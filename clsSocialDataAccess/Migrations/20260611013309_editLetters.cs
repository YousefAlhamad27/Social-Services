using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class editLetters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Admins_adminID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Users_userID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Volunteers_volunteerApplicationID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerProofImages_VolunteerApplications_VolunteerApplicationEntityvolunteerApplicationID",
                table: "VolunteerProofImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Users_userID",
                table: "Volunteers");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Volunteers",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Volunteers_userID",
                table: "Volunteers",
                newName: "IX_Volunteers_UserID");

            migrationBuilder.RenameColumn(
                name: "VolunteerApplicationEntityvolunteerApplicationID",
                table: "VolunteerProofImages",
                newName: "VolunteerApplicationEntityVolunteerApplicationID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerProofImages_VolunteerApplicationEntityvolunteerApplicationID",
                table: "VolunteerProofImages",
                newName: "IX_VolunteerProofImages_VolunteerApplicationEntityVolunteerApplicationID");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "VolunteerApplications",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "adminID",
                table: "VolunteerApplications",
                newName: "AdminID");

            migrationBuilder.RenameColumn(
                name: "volunteerApplicationID",
                table: "VolunteerApplications",
                newName: "VolunteerApplicationID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerApplications_userID",
                table: "VolunteerApplications",
                newName: "IX_VolunteerApplications_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerApplications_adminID",
                table: "VolunteerApplications",
                newName: "IX_VolunteerApplications_AdminID");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Admins_AdminID",
                table: "VolunteerApplications",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "AdminID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Users_UserID",
                table: "VolunteerApplications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Volunteers_VolunteerApplicationID",
                table: "VolunteerApplications",
                column: "VolunteerApplicationID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerProofImages_VolunteerApplications_VolunteerApplicationEntityVolunteerApplicationID",
                table: "VolunteerProofImages",
                column: "VolunteerApplicationEntityVolunteerApplicationID",
                principalTable: "VolunteerApplications",
                principalColumn: "VolunteerApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Users_UserID",
                table: "Volunteers",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Admins_AdminID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Users_UserID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerApplications_Volunteers_VolunteerApplicationID",
                table: "VolunteerApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_VolunteerProofImages_VolunteerApplications_VolunteerApplicationEntityVolunteerApplicationID",
                table: "VolunteerProofImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Volunteers_Users_UserID",
                table: "Volunteers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Volunteers",
                newName: "userID");

            migrationBuilder.RenameIndex(
                name: "IX_Volunteers_UserID",
                table: "Volunteers",
                newName: "IX_Volunteers_userID");

            migrationBuilder.RenameColumn(
                name: "VolunteerApplicationEntityVolunteerApplicationID",
                table: "VolunteerProofImages",
                newName: "VolunteerApplicationEntityvolunteerApplicationID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerProofImages_VolunteerApplicationEntityVolunteerApplicationID",
                table: "VolunteerProofImages",
                newName: "IX_VolunteerProofImages_VolunteerApplicationEntityvolunteerApplicationID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "VolunteerApplications",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "AdminID",
                table: "VolunteerApplications",
                newName: "adminID");

            migrationBuilder.RenameColumn(
                name: "VolunteerApplicationID",
                table: "VolunteerApplications",
                newName: "volunteerApplicationID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerApplications_UserID",
                table: "VolunteerApplications",
                newName: "IX_VolunteerApplications_userID");

            migrationBuilder.RenameIndex(
                name: "IX_VolunteerApplications_AdminID",
                table: "VolunteerApplications",
                newName: "IX_VolunteerApplications_adminID");

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Admins_adminID",
                table: "VolunteerApplications",
                column: "adminID",
                principalTable: "Admins",
                principalColumn: "AdminID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Users_userID",
                table: "VolunteerApplications",
                column: "userID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerApplications_Volunteers_volunteerApplicationID",
                table: "VolunteerApplications",
                column: "volunteerApplicationID",
                principalTable: "Volunteers",
                principalColumn: "VolunteerID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VolunteerProofImages_VolunteerApplications_VolunteerApplicationEntityvolunteerApplicationID",
                table: "VolunteerProofImages",
                column: "VolunteerApplicationEntityvolunteerApplicationID",
                principalTable: "VolunteerApplications",
                principalColumn: "volunteerApplicationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Volunteers_Users_userID",
                table: "Volunteers",
                column: "userID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
