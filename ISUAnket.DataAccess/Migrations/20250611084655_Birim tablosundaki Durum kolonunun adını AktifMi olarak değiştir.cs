using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BirimtablosundakiDurumkolonununadınıAktifMiolarakdeğiştir : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Durum",
                table: "Birimler",
                newName: "AktifMi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AktifMi",
                table: "Birimler",
                newName: "Durum");
        }
    }
}
