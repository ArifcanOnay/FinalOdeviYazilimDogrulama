using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddHakkimdaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hakkimda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unvan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilResmiUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EgitimBaslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EgitimAciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeneyimBaslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeneyimAciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkiyeBaslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkiyeAciklama = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timeline1Yil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timeline1Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timeline1Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timeline2Yil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timeline2Baslik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timeline2Aciklama = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hakkimda", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hakkimda");
        }
    }
}
