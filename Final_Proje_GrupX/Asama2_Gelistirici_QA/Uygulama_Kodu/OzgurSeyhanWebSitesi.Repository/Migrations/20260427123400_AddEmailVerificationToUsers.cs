using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailVerificationToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailDogrulamaTarihi",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailDogrulamaToken",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailDogrulamaTokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailDogrulandiMi",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailDogrulamaToken",
                table: "Users",
                column: "EmailDogrulamaToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailDogrulamaToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailDogrulamaTarihi",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailDogrulamaToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailDogrulamaTokenExpiry",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailDogrulandiMi",
                table: "Users");
        }
    }
}
