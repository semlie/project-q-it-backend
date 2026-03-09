using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFirst.Migrations
{
    /// <inheritdoc />
    public partial class FixTeacherClassOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('Users', 'ClassId') IS NULL
    ALTER TABLE [Users] ADD [ClassId] int NULL;
");

            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[TeacherClass]', N'U') IS NULL
BEGIN
    CREATE TABLE [TeacherClass](
        [TeacherClassId] int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [TeacherId] int NOT NULL,
        [ClassId] int NOT NULL
    );
END
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_ClassId' AND object_id = OBJECT_ID('Users'))
    CREATE INDEX [IX_Users_ClassId] ON [Users]([ClassId]);
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Users_Classes_ClassId')
    ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Classes_ClassId]
    FOREIGN KEY([ClassId]) REFERENCES [Classes]([ClassId]);
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'SchoolId') IS NULL
    ALTER TABLE [Course] ADD [SchoolId] int NULL;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'UserId') IS NOT NULL
   AND COL_LENGTH('Course', 'SchoolId') IS NOT NULL
    UPDATE [Course] SET [SchoolId] = [UserId] WHERE [SchoolId] IS NULL;
");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Course_Users_UserId')
    ALTER TABLE [Course] DROP CONSTRAINT [FK_Course_Users_UserId];
");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Course_UserId' AND object_id = OBJECT_ID('Course'))
    DROP INDEX [IX_Course_UserId] ON [Course];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'UserId') IS NOT NULL
    ALTER TABLE [Course] DROP COLUMN [UserId];
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Course_SchoolId' AND object_id = OBJECT_ID('Course'))
    CREATE INDEX [IX_Course_SchoolId] ON [Course]([SchoolId]);
");

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Course_School_SchoolId')
    ALTER TABLE [Course] ADD CONSTRAINT [FK_Course_School_SchoolId]
    FOREIGN KEY ([SchoolId]) REFERENCES [School]([SchoolId]);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Users_Classes_ClassId')
    ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Classes_ClassId];
");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Users_ClassId' AND object_id = OBJECT_ID('Users'))
    DROP INDEX [IX_Users_ClassId] ON [Users];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Users', 'ClassId') IS NOT NULL
    ALTER TABLE [Users] DROP COLUMN [ClassId];
");

            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[TeacherClass]', N'U') IS NOT NULL
    DROP TABLE [TeacherClass];
");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Course_School_SchoolId')
    ALTER TABLE [Course] DROP CONSTRAINT [FK_Course_School_SchoolId];
");

            migrationBuilder.Sql(@"
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Course_SchoolId' AND object_id = OBJECT_ID('Course'))
    DROP INDEX [IX_Course_SchoolId] ON [Course];
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'UserId') IS NULL
    ALTER TABLE [Course] ADD [UserId] int NULL;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'SchoolId') IS NOT NULL
   AND COL_LENGTH('Course', 'UserId') IS NOT NULL
    UPDATE [Course] SET [UserId] = [SchoolId] WHERE [UserId] IS NULL;
");

            migrationBuilder.Sql(@"
IF COL_LENGTH('Course', 'SchoolId') IS NOT NULL
    ALTER TABLE [Course] DROP COLUMN [SchoolId];
");
        }
    }
}
