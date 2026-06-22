using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class edit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
           name: "Status",
           table: "ServiceApplications",
           type: "tinyint",
           nullable: false,
           defaultValue: (byte)0);


            migrationBuilder.Sql("UPDATE ServiceApplications SET Status = 2 WHERE Accepted = 1");

            // 2. Set Rejected users (0 but has a message) to Status 3
            migrationBuilder.Sql("UPDATE ServiceApplications SET Status = 3 WHERE Accepted = 0 AND AcceptanceMessage IS NOT NULL");

            // 3. Set Pending users (0 and no message) to Status 1
            migrationBuilder.Sql("UPDATE ServiceApplications SET Status = 1 WHERE Accepted = 0 AND AcceptanceMessage IS NULL");

            // 3. SAFELY DROP the old column now that the data is saved
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "ServiceApplications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ServiceApplications");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "ServiceApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
