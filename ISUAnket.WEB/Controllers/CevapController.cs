using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace ISUAnket.WEB.Controllers
{
    public class CevapController : Controller
    {
        private readonly ICevapService _cevapService;

        public CevapController(ICevapService cevapService)
        {
            _cevapService = cevapService;
        }

        public async Task<IActionResult> CevapListesi(int page=1,int pageSize=10)
        {
            var sonuc = await _cevapService.GetAllServiceAsync(x => x.AktifMi == true, x => x.Soru,x=>x.Soru.Anket, x => x.Birim);

            int totalCount = sonuc.Count;

            var sayfaCevaplari = sonuc
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

            ViewData["TotalCount"] = totalCount;
            ViewData["PageSize"] = pageSize;
            ViewData["CurrentPage"] = page;

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
                    page.Margin(30);
                    page.Header().Text("Cevap Listesi").FontSize(20).SemiBold().AlignCenter();
                    page.Content().Table(table =>
                    {
                        // Kolon sayısını belirliyoruz
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(40); // Sıra No
                            columns.RelativeColumn(); // Anket
                            columns.RelativeColumn(); // Soru
                            columns.RelativeColumn(); // Cevap
                            columns.RelativeColumn(); // Birim
                            columns.ConstantColumn(100); // Tarih
                        });

                        // Tablo başlıkları
                        table.Header(header =>
                        {
                            header.Cell().Text("No").SemiBold();
                            header.Cell().Text("Anket").SemiBold();
                            header.Cell().Text("Soru").SemiBold();
                            header.Cell().Text("Cevap").SemiBold();
                            header.Cell().Text("Birim").SemiBold();
                            header.Cell().Text("Tarih").SemiBold();
                        });

                        int sayac = 1;
                        foreach (var item in cevaplar)
                        {
                            table.Cell().Text(sayac.ToString());
                            table.Cell().Text(item.Soru?.Anket?.Ad ?? "");
                            table.Cell().Text(item.Soru?.SoruMetni ?? "");
                            table.Cell().Text(item.VerilenCevap ?? "");
                            table.Cell().Text(item.Birim?.Ad ?? "");
                            table.Cell().Text(item.CevapTarihi.ToString("yyyy-MM-dd"));
                            sayac++;
                        }
                    });
                });
            });

            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
