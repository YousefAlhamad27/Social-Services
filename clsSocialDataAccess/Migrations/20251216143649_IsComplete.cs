using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace clsSocialServicesDataAccess.Migrations
{
    /// <inheritdoc />
    public partial class IsComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

     

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        
        }
    }
}
