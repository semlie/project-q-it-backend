using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class _20260302113000_RestrictQuestionLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.Sql("UPDATE [Question] SET [Level] = 1 WHERE [Level] NOT IN (1, 2, 3);");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Question_Level",
                table: "Question",
                sql: "[Level] IN (1, 2, 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Question_Level",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Question");
        }
    }
}
