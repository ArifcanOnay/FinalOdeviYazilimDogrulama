using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OzgurSeyhanWebSitesi.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixHakkimdaDateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Hakkimda");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Hakkimda",
                newName: "UpdateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Hakkimda",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Hakkimda");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Hakkimda",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Hakkimda",
                type: "datetime2",
                nullable: true);
        }
    }
}
