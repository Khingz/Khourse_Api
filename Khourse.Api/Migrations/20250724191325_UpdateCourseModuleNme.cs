using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Khourse.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseModuleNme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k__course_users_author_id",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "f_k__module__course_course_id",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "p_k__module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "p_k__course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "modules");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "courses");

            migrationBuilder.RenameIndex(
                name: "i_x__module_course_id",
                table: "modules",
                newName: "i_x_modules_course_id");

            migrationBuilder.RenameIndex(
                name: "i_x__course_author_id",
                table: "courses",
                newName: "i_x_courses_author_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_modules",
                table: "modules",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k_courses",
                table: "courses",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_courses_users_author_id",
                table: "courses",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k_modules_courses_course_id",
                table: "modules",
                column: "course_id",
                principalTable: "courses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "f_k_courses_users_author_id",
                table: "courses");

            migrationBuilder.DropForeignKey(
                name: "f_k_modules_courses_course_id",
                table: "modules");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_modules",
                table: "modules");

            migrationBuilder.DropPrimaryKey(
                name: "p_k_courses",
                table: "courses");

            migrationBuilder.RenameTable(
                name: "modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "i_x_modules_course_id",
                table: "Module",
                newName: "i_x__module_course_id");

            migrationBuilder.RenameIndex(
                name: "i_x_courses_author_id",
                table: "Course",
                newName: "i_x__course_author_id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k__module",
                table: "Module",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "p_k__course",
                table: "Course",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k__course_users_author_id",
                table: "Course",
                column: "author_id",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "f_k__module__course_course_id",
                table: "Module",
                column: "course_id",
                principalTable: "Course",
                principalColumn: "id");
        }
    }
}
