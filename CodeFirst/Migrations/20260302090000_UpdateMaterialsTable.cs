using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace CodeFirst.Migrations
{
    [DbContext(typeof(Models.BDQit))]
    [Migration("20260302090000_UpdateMaterialsTable")]
    public partial class UpdateMaterialsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Materials");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MatDescription",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatLink",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MatName",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MatDescription",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MatLink",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "MatName",
                table: "Materials");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
