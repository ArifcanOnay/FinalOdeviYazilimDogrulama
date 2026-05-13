using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaylistTableFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaylistId",
                table: "YoutubeVideos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaylistId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Baslik = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LastSyncDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OgretmenId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Playlists_Ogretmenler_OgretmenId",
                        column: x => x.OgretmenId,
                        principalTable: "Ogretmenler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeVideos_PlaylistId",
                table: "YoutubeVideos",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_OgretmenId",
                table: "Playlists",
                column: "OgretmenId");

            migrationBuilder.AddForeignKey(
                name: "FK_YoutubeVideos_Playlists_PlaylistId",
                table: "YoutubeVideos",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YoutubeVideos_Playlists_PlaylistId",
                table: "YoutubeVideos");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropIndex(
                name: "IX_YoutubeVideos_PlaylistId",
                table: "YoutubeVideos");

            migrationBuilder.DropColumn(
                name: "PlaylistId",
                table: "YoutubeVideos");
        }
    }
}
