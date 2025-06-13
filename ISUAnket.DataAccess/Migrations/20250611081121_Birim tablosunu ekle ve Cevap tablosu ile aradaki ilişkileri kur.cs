using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class BirimtablosunuekleveCevaptablosuilearadakiilişkilerikur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birim",
                table: "Cevaplar");

            migrationBuilder.AlterColumn<string>(
                name: "RolAdi",
                table: "Roller",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "VerilenCevap",
                table: "Cevaplar",
                type: "nvarchar(1500)",
                maxLength: 1500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(600)",
                oldMaxLength: 600);

            migrationBuilder.AddColumn<int>(
                name: "BirimId",
                table: "Cevaplar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Birimler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Durum = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birimler", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_BirimId",
                table: "Cevaplar",
                column: "BirimId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cevaplar_Birimler_BirimId",
                table: "Cevaplar",
                column: "BirimId",
                principalTable: "Birimler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cevaplar_Birimler_BirimId",
                table: "Cevaplar");

            migrationBuilder.DropTable(
                name: "Birimler");

            migrationBuilder.DropIndex(
                name: "IX_Cevaplar_BirimId",
                table: "Cevaplar");

            migrationBuilder.DropColumn(
                name: "BirimId",
                table: "Cevaplar");

            migrationBuilder.AlterColumn<string>(
                name: "RolAdi",
                table: "Roller",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "VerilenCevap",
                table: "Cevaplar",
                type: "nvarchar(600)",
                maxLength: 600,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldMaxLength: 1500);

            migrationBuilder.AddColumn<string>(
                name: "Birim",
                table: "Cevaplar",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
