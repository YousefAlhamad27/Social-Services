using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CLEAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          


            migrationBuilder.CreateTable(
                name: "VolunteerApplications",
                columns: table => new
                {
                    VolunteerApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    AdminID = table.Column<int>(type: "int", nullable: true),
                    IdImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerApplications", x => x.VolunteerApplicationID);
                    table.ForeignKey(
                        name: "FK_VolunteerApplications_Admins_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Admins",
                        principalColumn: "AdminID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VolunteerApplications_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

         

            migrationBuilder.CreateTable(
                name: "VolunteerProofImages",
                columns: table => new
                {
                    ImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolunteerApplicationID = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolunteerProofImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_VolunteerProofImages_VolunteerApplications_VolunteerApplicationID",
                        column: x => x.VolunteerApplicationID,
                        principalTable: "VolunteerApplications",
                        principalColumn: "VolunteerApplicationID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    VolunteerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    VolunteerApplicationID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PointsCount = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.VolunteerID);
                    table.ForeignKey(
                        name: "FK_Volunteers_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Volunteers_VolunteerApplications_VolunteerApplicationID",
                        column: x => x.VolunteerApplicationID,
                        principalTable: "VolunteerApplications",
                        principalColumn: "VolunteerApplicationID",
                        onDelete: ReferentialAction.Restrict);
                });

          

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerApplications_AdminID",
                table: "VolunteerApplications",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerApplications_UserID",
                table: "VolunteerApplications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerProofImages_VolunteerApplicationID",
                table: "VolunteerProofImages",
                column: "VolunteerApplicationID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_UserID",
                table: "Volunteers",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Volunteers_VolunteerApplicationID",
                table: "Volunteers",
                column: "VolunteerApplicationID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ServiceApplications");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "VolunteerProofImages");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "VolunteerApplications");

            migrationBuilder.DropTable(
                name: "Counties");

            migrationBuilder.DropTable(
                name: "PostTypes");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
