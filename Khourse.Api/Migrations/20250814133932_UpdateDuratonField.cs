using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khourse.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDuratonField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimated_duration_mins",
                table: "modules");

            migrationBuilder.RenameColumn(
                name: "video_duration_mins",
                table: "lessons",
                newName: "duration_mins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "duration_mins",
                table: "lessons",
                newName: "video_duration_mins");

            migrationBuilder.AddColumn<int>(
                name: "estimated_duration_mins",
                table: "modules",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
