using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class addCovertypeStoredProc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverTypes AS BEGIN SELECT * FROM CoverTypes END");
            migrationBuilder.Sql(@"CREATE PROC usp_GetCoverType @Id int AS BEGIN SELECT * FROM dbo.CoverTypes WHERE (Id = @Id) END");
            migrationBuilder.Sql(@"CREATE PROC usp_UpdateCoverType @Id int, @Name varchar(100) AS BEGIN UPDATE CoverTypes SET Name = @Name WHERE Id=@Id END");
            migrationBuilder.Sql(@"CREATE PROC usp_DeleteCoverType @Id int AS BEGIN DELETE FROM CoverTypes WHERE Id = @Id END");
            migrationBuilder.Sql(@"CREATE PROC usp_CreateCoverType @Name varchar(100) AS BEGIN INSERT INTO CoverTypes(Name) values(@Name) END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverTypes");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_GetCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_UpdateCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_DeleteCoverType");
            migrationBuilder.Sql(@"DROP PROCEDURE usp_CreateCoverType");
        }
    }
}
