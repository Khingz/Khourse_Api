using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khourse.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveModuleCountFromCOurse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_module",
                table: "courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_module",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
