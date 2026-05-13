using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFlowPuzzleRelativeClauseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowPuzzleRelativeClauses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourcePairEn = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: false),
                    RelativeClauseEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TurkishTranslation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SourcePairTr = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    RelativeClauseTr = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowPuzzleRelativeClauses", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowPuzzleRelativeClauses");
        }
    }
}
