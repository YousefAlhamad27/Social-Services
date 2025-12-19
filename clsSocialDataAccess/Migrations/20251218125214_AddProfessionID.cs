using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialServicesDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProfessionID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "ProfessionID",
                table: "Posts",
                type: "int",
                nullable: false,
                
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
       
        }
    }
}
