using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveHakkimdaExtraFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeneyimAciklama",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "DeneyimBaslik",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "EgitimAciklama",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "EgitimBaslik",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline1Aciklama",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline1Baslik",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline1Yil",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline2Aciklama",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline2Baslik",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "Timeline2Yil",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "TurkiyeAciklama",
                table: "Hakkimda");

            migrationBuilder.DropColumn(
                name: "TurkiyeBaslik",
                table: "Hakkimda");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeneyimAciklama",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeneyimBaslik",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EgitimAciklama",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EgitimBaslik",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline1Aciklama",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline1Baslik",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline1Yil",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline2Aciklama",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline2Baslik",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timeline2Yil",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TurkiyeAciklama",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TurkiyeBaslik",
                table: "Hakkimda",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
