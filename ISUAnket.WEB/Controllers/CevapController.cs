using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class CevapController : Controller
    {
        private readonly ICevapService _cevapService;
        private readonly IAnketService _anketService;
        private readonly IBirimService _birimService;

        public CevapController(ICevapService cevapService, IAnketService anketService, IBirimService birimService)
        {
            _cevapService = cevapService;
            _anketService = anketService;
            _birimService = birimService;
        }

        //public async Task<IActionResult> CevapListesi(int page=1,int pageSize=10)
        //{
        //    var sonuc = await _cevapService.GetAllServiceAsync(x => x.AktifMi == true, x => x.Soru,x=>x.Soru.Anket, x => x.Birim);

        //    int totalCount = sonuc.Count;

        //    var sayfaCevaplari = sonuc
        //                        .Skip((page - 1) * pageSize)
        //                        .Take(pageSize)
        //                        .ToList();

        //    ViewData["TotalCount"] = totalCount;
        //    ViewData["PageSize"] = pageSize;
        //    ViewData["CurrentPage"] = page;

        //    return View(sayfaCevaplari);
        //}

        //Filtreleme için kullanıldı
        public async Task<IActionResult> CevapListesi(int page = 1, int pageSize = 10, int? anketId = null, List<int> birimId = null)
        {
            var filtreliCevaplar = await _cevapService.GetAllServiceAsync(
                x => x.AktifMi == true &&
                     (!anketId.HasValue || x.Soru.AnketId == anketId) &&
                     (birimId == null || birimId.Count == 0 || birimId.Contains(x.BirimId)),
                x => x.Soru, x => x.Soru.Anket, x => x.Birim
            );

            var tumBirimler = await _birimService.GetListAllServiceAsync();
            var tumAnketler = await _anketService.GetAllServiceAsync();

            ViewBag.Birimler = new MultiSelectList(tumBirimler.OrderBy(b => b.Ad), "Id", "Ad", birimId);
            ViewBag.AktifAnketler = new SelectList(tumAnketler.OrderBy(a => a.Ad), "Id", "Ad", anketId);

            int totalCount = filtreliCevaplar.Count;
            var sayfaCevaplari = filtreliCevaplar
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["TotalCount"] = totalCount;
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = page;
            ViewData["SeciliAnketId"] = anketId;
            ViewData["SeciliBirimIdListesi"] = birimId;

            return View(sayfaCevaplari);
        }

        [HttpPost]
        public async Task<IActionResult> ExportToExcel()
        {
            var cevaplar = await _cevapService.GetAllServiceAsync(x => x.AktifMi == true, x => x.Soru, x => x.Soru.Anket, x => x.Birim);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Cevaplar");

                // Başlıklar
                worksheet.Cells[1, 1].Value = "Sıra No";
                worksheet.Cells[1, 2].Value = "Anket";
                worksheet.Cells[1, 3].Value = "Soru";
                worksheet.Cells[1, 4].Value = "Verilen Cevap";
                worksheet.Cells[1, 5].Value = "Birim";
                worksheet.Cells[1, 6].Value = "Cevap Tarihi";

                int row = 2;
                int sayac = 1;

                foreach (var item in cevaplar)
                {
                    worksheet.Cells[row, 1].Value = sayac;
                    worksheet.Cells[row, 2].Value = item.Soru?.Anket?.Ad;
                    worksheet.Cells[row, 3].Value = item.Soru?.SoruMetni;
                    worksheet.Cells[row, 4].Value = item.VerilenCevap;
                    worksheet.Cells[row, 5].Value = item.Birim?.Ad;
                    worksheet.Cells[row, 6].Value = item.CevapTarihi.ToString("yyyy-MM-dd HH:mm");
                    row++;
                    sayac++;
                }

                // Genişlik ayarı (opsiyonel)
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"CevapListesi-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportToPdf()
        {
            var cevaplar = await _cevapService.GetAllServiceAsync(x => x.AktifMi == true, x => x.Soru, x => x.Soru.Anket, x => x.Birim);

            byte[] pdfBytes = GeneratePdf(cevaplar);

            string pdfName = $"CevapListesi-{DateTime.Now:yyyyMMddHHmmss}.pdf";

            return File(pdfBytes, "application/pdf", pdfName);
        }


        private byte[] GeneratePdf(List<Cevap> cevaplar)
        {
            using var stream = new MemoryStream();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(0); // Sayfa kenar boşluklarını sıfırla

                    // Başlık
                    page.Header().Element(header =>
                    {
                        header
                            .PaddingVertical(10)
                            .Background("#F0F0F0")
                            .AlignCenter()
                            .Text(t => t.Span("Cevap Listesi").FontSize(20).Bold());
                    });

                    // İçerik
                    page.Content().Padding(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40);       // No
                            columns.RelativeColumn(1);        // Anket
                            columns.RelativeColumn(3);        // Soru
                            columns.RelativeColumn(2);        // Cevap
                            columns.RelativeColumn(2);        // Birim
                            columns.ConstantColumn(100);      // Tarih
                        });

                        // Tablo Başlıkları
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text(t => t.Span("No").Bold());
                            header.Cell().Element(CellStyle).Text(t => t.Span("Anket").Bold());
                            header.Cell().Element(CellStyle).Text(t => t.Span("Soru").Bold());
                            header.Cell().Element(CellStyle).Text(t => t.Span("Cevap").Bold());
                            header.Cell().Element(CellStyle).Text(t => t.Span("Birim").Bold());
                            header.Cell().Element(CellStyle).Text(t => t.Span("Tarih").Bold());
                        });

                        // Tablo Satırları
                        int sayac = 1;
                        foreach (var item in cevaplar)
                        {
                            table.Cell().Element(CellStyle).Text(sayac.ToString());
                            table.Cell().Element(CellStyle).Text(item.Soru?.Anket?.Ad ?? "");
                            table.Cell().Element(CellStyle).Text(item.Soru?.SoruMetni ?? "");
                            table.Cell().Element(CellStyle).Text(item.VerilenCevap ?? "");
                            table.Cell().Element(CellStyle).Text(item.Birim?.Ad ?? "");
                            table.Cell().Element(CellStyle).Text(item.CevapTarihi.ToString("yyyy-MM-dd"));
                            sayac++;
                        }

                        // Hücre stil fonksiyonu (alt çizgi kaldırıldı)
                        static IContainer CellStyle(IContainer container) =>
                            container
                                .PaddingVertical(4)
                                .PaddingHorizontal(3)
                                .AlignMiddle();
                    });

                    // Alt bilgi (Sayfa numarası)
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Sayfa ").FontSize(10).Italic();
                        text.CurrentPageNumber().FontSize(10).Italic();
                        text.Span(" / ").FontSize(10).Italic();
                        text.TotalPages().FontSize(10).Italic();
                    });
                });
            });

            document.GeneratePdf(stream);
            return stream.ToArray();
        }


        //private byte[] GeneratePdf(List<Cevap> cevaplar)
        //{
        //    using var stream = new MemoryStream();

        //    var document = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin(20);
        //            page.Header().Text("Cevap Listesi").FontSize(20).Bold().AlignCenter();
        //            page.Content().Table(table =>
        //            {
        //                // Kolon sayısını belirliyoruz
        //                table.ColumnsDefinition(columns =>
        //                {
        //                    columns.ConstantColumn(40); // Sıra No
        //                    columns.RelativeColumn(1); // Anket
        //                    columns.RelativeColumn(4); // Soru
        //                    columns.RelativeColumn(1.5f); // Cevap
        //                    columns.RelativeColumn(2); // Birim
        //                    columns.ConstantColumn(100); // Tarih
        //                });

        //                // Tablo başlıkları
        //                table.Header(header =>
        //                {
        //                    header.Cell().Text("No").Bold();
        //                    header.Cell().Text("Anket").Bold();
        //                    header.Cell().Text("Soru").Bold();
        //                    header.Cell().Text("Cevap").Bold();
        //                    header.Cell().Text("Birim").Bold();
        //                    header.Cell().Text("Tarih").Bold();
        //                });

        //                int sayac = 1;
        //                foreach (var item in cevaplar)
        //                {
        //                    table.Cell().Text(sayac.ToString());
        //                    table.Cell().Text(item.Soru?.Anket?.Ad ?? "");
        //                    table.Cell().Text(item.Soru?.SoruMetni ?? "");
        //                    table.Cell().Text(item.VerilenCevap ?? "");
        //                    table.Cell().Text(item.Birim?.Ad ?? "");
        //                    table.Cell().Text(item.CevapTarihi.ToString("yyyy-MM-dd"));
        //                    sayac++;
        //                }
        //            });
        //        });
        //    });

        //    document.GeneratePdf(stream);
        //    return stream.ToArray();
        //}
    }
}
