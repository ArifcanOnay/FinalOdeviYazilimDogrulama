using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Sadece gerçekten eksik kolonlar ekleniyor
            // Podcasts.KapakResmi - ZATEN EKLENDİ, TEKRAR EKLENMEYECEK
            // Playlists.KategoriBaslik - ZATEN VAR
            // YoutubeVideos.Kategori ve KategoriBaslik - ZATEN VAR

            migrationBuilder.AddColumn<string>(
                name: "ResimUrl",
                table: "OzelDersler",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeVideoUrl",
                table: "OzelDersler",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Sadece eklenen kolonlar silinecek

            migrationBuilder.DropColumn(
                name: "ResimUrl",
                table: "OzelDersler");

            migrationBuilder.DropColumn(
                name: "YoutubeVideoUrl",
                table: "OzelDersler");
        }
    }
}
