using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ISUAnket.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ANKET");

            migrationBuilder.CreateTable(
                name: "Birimler",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birimler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roller",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RolAdi = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCKN = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Soyad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KulaniciAdi = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sifre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OturumAcikMi = table.Column<bool>(type: "boolean", nullable: false),
                    SonCikisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    RolId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_Roller_RolId",
                        column: x => x.RolId,
                        principalSchema: "ANKET",
                        principalTable: "Roller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Anketler",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Link = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    BaslangicTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AnketDurumu = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    OlusturanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    DuzenleyenKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    DuzenlenmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anketler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anketler_Kullanicilar_DuzenleyenKullaniciId",
                        column: x => x.DuzenleyenKullaniciId,
                        principalSchema: "ANKET",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Anketler_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalSchema: "ANKET",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sorular",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoruMetni = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SoruTipi = table.Column<int>(type: "integer", nullable: false),
                    VeriTipi = table.Column<int>(type: "integer", nullable: false),
                    SoruSecenekleri = table.Column<string>(type: "text", nullable: true),
                    OlusturanKullaniciId = table.Column<int>(type: "integer", nullable: false),
                    OlusturmaTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DuzenleyenKullaniciId = table.Column<int>(type: "integer", nullable: true),
                    DuzenlenmeTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    ZorunluMu = table.Column<bool>(type: "boolean", nullable: false),
                    AnketId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorular_Anketler_AnketId",
                        column: x => x.AnketId,
                        principalSchema: "ANKET",
                        principalTable: "Anketler",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sorular_Kullanicilar_DuzenleyenKullaniciId",
                        column: x => x.DuzenleyenKullaniciId,
                        principalSchema: "ANKET",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sorular_Kullanicilar_OlusturanKullaniciId",
                        column: x => x.OlusturanKullaniciId,
                        principalSchema: "ANKET",
                        principalTable: "Kullanicilar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cevaplar",
                schema: "ANKET",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VerilenCevap = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CevapTarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    AktifMi = table.Column<bool>(type: "boolean", nullable: false),
                    SoruId = table.Column<int>(type: "integer", nullable: false),
                    BirimId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cevaplar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cevaplar_Birimler_BirimId",
                        column: x => x.BirimId,
                        principalSchema: "ANKET",
                        principalTable: "Birimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cevaplar_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalSchema: "ANKET",
                        principalTable: "Sorular",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_DuzenleyenKullaniciId",
                schema: "ANKET",
                table: "Anketler",
                column: "DuzenleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Anketler_OlusturanKullaniciId",
                schema: "ANKET",
                table: "Anketler",
                column: "OlusturanKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_BirimId",
                schema: "ANKET",
                table: "Cevaplar",
                column: "BirimId");

            migrationBuilder.CreateIndex(
                name: "IX_Cevaplar_SoruId",
                schema: "ANKET",
                table: "Cevaplar",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_RolId",
                schema: "ANKET",
                table: "Kullanicilar",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_AnketId",
                schema: "ANKET",
                table: "Sorular",
                column: "AnketId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_DuzenleyenKullaniciId",
                schema: "ANKET",
                table: "Sorular",
                column: "DuzenleyenKullaniciId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_OlusturanKullaniciId",
                schema: "ANKET",
                table: "Sorular",
                column: "OlusturanKullaniciId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cevaplar",
                schema: "ANKET");

            migrationBuilder.DropTable(
                name: "Birimler",
                schema: "ANKET");

            migrationBuilder.DropTable(
                name: "Sorular",
                schema: "ANKET");

            migrationBuilder.DropTable(
                name: "Anketler",
                schema: "ANKET");

            migrationBuilder.DropTable(
                name: "Kullanicilar",
                schema: "ANKET");

            migrationBuilder.DropTable(
                name: "Roller",
                schema: "ANKET");
        }
    }
}
