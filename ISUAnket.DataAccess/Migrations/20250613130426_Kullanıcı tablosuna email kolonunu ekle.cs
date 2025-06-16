using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Kullanıcıtablosunaemailkolonunuekle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Kullanicilar",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Kullanicilar");
        }
    }
}
