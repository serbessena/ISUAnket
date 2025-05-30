using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class KullanıcıtablosunaSonCikisTarihiveOturumAcikMikolonlarınıekle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OturumAcikMi",
                table: "Kullanicilar",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SonCikisTarihi",
                table: "Kullanicilar",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OturumAcikMi",
                table: "Kullanicilar");

            migrationBuilder.DropColumn(
                name: "SonCikisTarihi",
                table: "Kullanicilar");
        }
    }
}
