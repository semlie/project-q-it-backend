using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddTestAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "TestResults",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TestAttempts",
                columns: table => new
                {
                    TestAttemptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    SelectedAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChapterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAttempts", x => x.TestAttemptId);
                    table.ForeignKey(
                        name: "FK_TestAttempts_Chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapter",
                        principalColumn: "ChapterId");
                    table.ForeignKey(
                        name: "FK_TestAttempts_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestAttempts_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_ChapterId",
                table: "TestResults",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_ChapterId",
                table: "TestAttempts",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_QuestionId",
                table: "TestAttempts",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAttempts_StudentId",
                table: "TestAttempts",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Chapter_ChapterId",
                table: "TestResults",
                column: "ChapterId",
                principalTable: "Chapter",
                principalColumn: "ChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Chapter_ChapterId",
                table: "TestResults");

            migrationBuilder.DropTable(
                name: "TestAttempts");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_ChapterId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "TestResults");
        }
    }
}
