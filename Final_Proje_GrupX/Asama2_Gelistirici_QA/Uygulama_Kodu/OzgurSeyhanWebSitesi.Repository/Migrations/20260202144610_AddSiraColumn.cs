using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddSiraColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "YoutubeVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "Podcasts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Sira",
                table: "OzelDersler",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sira",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "Podcasts");

            migrationBuilder.DropColumn(
                name: "Sira",
                table: "OzelDersler");
        }
    }
}
