using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VeritabanınıOlustur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TCKN = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KulaniciAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Roller_RolId",
                        column: x => x.RolId,
                        principalTable: "Roller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Anketler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnketDurumu = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    DuzenleyenKullaniciId = table.Column<int>(type: "int", nullable: true),
                    DuzenlenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anketler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anketler_Kullanicilar_DuzenleyenKullaniciId",
                        column: x => x.DuzenleyenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Anketler_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoruMetni = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SoruTipi = table.Column<int>(type: "int", nullable: false),
                    SoruSecenekleri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "int", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DuzenleyenKullaniciId = table.Column<int>(type: "int", nullable: true),
                    DuzenlenmeTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    ZorunluMu = table.Column<bool>(type: "bit", nullable: true),
                    AnketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorular_Anketler_AnketId",
                        column: x => x.AnketId,
                        principalTable: "Anketler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sorular_Kullanicilar_DuzenleyenKullaniciId",
                        column: x => x.DuzenleyenKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sorular_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cevaplar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Birim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerilenCevap = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    CevapTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false),
                    SoruId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cevaplar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cevaplar_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalTable: "Sorular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_DuzenleyenKullaniciId",
                table: "Anketler",
                column: "DuzenleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_OlusturanKullaniciId",
                table: "Anketler",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_SoruId",
                table: "Cevaplar",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_RolId",
                table: "Kullanicilar",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_AnketId",
                table: "Sorular",
                column: "AnketId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_DuzenleyenKullaniciId",
                table: "Sorular",
                column: "DuzenleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_OlusturanKullaniciId",
                table: "Sorular",
                column: "OlusturanKullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cevaplar");

            migrationBuilder.DropTable(
                name: "Sorular");

            migrationBuilder.DropTable(
                name: "Anketler");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Roller");
        }
    }
}
