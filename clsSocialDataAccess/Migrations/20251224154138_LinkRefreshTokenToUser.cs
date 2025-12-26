using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialServicesDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class LinkRefreshTokenToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // It does NOT say "AddColumn"
            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId"); // Uses existing column

            migrationBuilder.AddForeignKey( // Adds the RULE, not the column
                name: "FK_Tokens_Users_UserId",
                table: "Tokens",
                column: "UserId", // Uses existing column
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
