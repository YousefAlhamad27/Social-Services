using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class zzzdasd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccomplishedServiceApplicationsCount",
                table: "Volunteers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccomplishedServiceApplicationsCount",
                table: "Volunteers");
        }
    }
}
