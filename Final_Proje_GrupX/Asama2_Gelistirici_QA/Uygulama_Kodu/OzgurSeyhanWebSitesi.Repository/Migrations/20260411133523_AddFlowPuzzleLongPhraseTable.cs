using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFlowPuzzleLongPhraseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowPuzzleLongPhrases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadNounEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OfPhraseEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DetailPhraseEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ContextPhraseEn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FullPhraseEn = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    TurkishTranslation = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowPuzzleLongPhrases", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowPuzzleLongPhrases");
        }
    }
}
