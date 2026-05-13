using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFlowPuzzleSentenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowPuzzleSentences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Preposition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeterminerSingular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeterminerPlural = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NounId = table.Column<int>(type: "int", nullable: false),
                    TurkishSingular = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkishPlural = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowPuzzleSentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowPuzzleSentences_FlowPuzzleNouns_NounId",
                        column: x => x.NounId,
                        principalTable: "FlowPuzzleNouns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlowPuzzleSentences_NounId",
                table: "FlowPuzzleSentences",
                column: "NounId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowPuzzleSentences");
        }
    }
}
