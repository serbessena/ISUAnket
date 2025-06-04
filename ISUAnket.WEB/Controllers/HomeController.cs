using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ISUAnket.WEB.Models;
using ISUAnket.Business.Interfaces;
using System.Text.Json;
namespace ISUAnket.WEB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISoruService _soruService;

    public HomeController(ILogger<HomeController> logger, ISoruService soruService)
    {
        _logger = logger;
        _soruService = soruService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Kvkk(int anketId = 1)
    {
        ViewBag.AnketId = anketId;
        return View();
    }

    [HttpPost]
    public IActionResult KvkkOnayla(bool KvkkOnay, int anketId)
    {
        if (!KvkkOnay)
        {
            TempData["Hata"] = "Devam etmek için KVKK onay kutusunu işaretlemeniz gerekir.";
            return RedirectToAction("KVKK");
        }

        HttpContext.Session.SetString("KvkkOnay", "true");

        return RedirectToAction("AnketDoldur", new { anketId = anketId, sayfa = 1 });
    }

    public async Task<IActionResult> AnketDoldur(int anketId, int sayfa = 1)
    {
        var kvkk = HttpContext.Session.GetString("KvkkOnay");
        if (kvkk != "true")
        {
            return RedirectToAction("Kvkk", new { anketId });
        }

        var sorular = await _soruService.GetAllServiceAsync();

        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        int toplamSayfa = sorular.Count;
        if (sayfa < 1) sayfa = 1;
        if (sayfa > toplamSayfa) sayfa = toplamSayfa;

        var seciliSoru = sorular[sayfa - 1];

        var cevaplar = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplar)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

        ViewBag.Cevaplar = cevapSozluk;
        ViewBag.Sayfa = sayfa;
        ViewBag.ToplamSayfa = toplamSayfa;
        ViewBag.AnketId = anketId;

        return View(seciliSoru);
    }

    [HttpPost]
    public async Task<IActionResult> AnketDoldur(int anketId, int sayfa, IFormCollection form)
    {
        // Cevapları Session'dan al
        var cevaplar = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplar)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

        // Anket sorularını getir
        var sorular = await _soruService.GetAllServiceAsync();
        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        int toplamSayfa = sorular.Count;
        var seciliSoru = sorular[sayfa - 1]; // Sayfadaki soru

        // Cevap formdan alınıyor
        string cevapKey = $"Cevap_{seciliSoru.Id}";
        string cevapDegeri = form[cevapKey];

        // Eğer zorunluysa ve boşsa uyarı ver
        if (seciliSoru.ZorunluMu && string.IsNullOrWhiteSpace(cevapDegeri))
        {
            TempData["Hata"] = "Lütfen bu soruyu cevaplayınız.";
            return RedirectToAction("AnketDoldur", new { anketId, sayfa });
        }

        // Cevabı Session'a ekle
        cevapSozluk[cevapKey] = string.Join(";", form[cevapKey]);
        HttpContext.Session.SetString("Cevaplar", System.Text.Json.JsonSerializer.Serialize(cevapSozluk));

        string islem = form["islem"];

        if (islem == "bitir")
        {
            return RedirectToAction("AnketSonuc", new { anketId });
        }

        return RedirectToAction("AnketDoldur", new { anketId, sayfa = sayfa + 1 });
    }

    public async Task<IActionResult> AnketSonuc(int anketId)
    {
        var cevaplar = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplar)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

        var sorular = await _soruService.GetAllServiceAsync();
        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        var sonucListesi = new List<(string SoruMetni, string Cevap)>(); // ViewModel gibi

        foreach (var soru in sorular)
        {
            var key = $"Cevap_{soru.Id}";
            if (cevapSozluk.TryGetValue(key, out var cevap) && !string.IsNullOrWhiteSpace(cevap))
            {
                sonucListesi.Add((soru.SoruMetni, cevap));
            }
            else
            {
                string cevapMetni = soru.ZorunluMu ? "Bu soru zorunlu ancak cevap girilmemiş." : "Cevap zorunlu olmadığı için cevap girilmedi.";
                sonucListesi.Add((soru.SoruMetni, cevapMetni));
            }
        }

        HttpContext.Session.Remove("Cevaplar");

        return View(sonucListesi);
    }

    /*******************************  Anket Soru sayfası sayfalar arası geçiş yapılıyor ama sorunun cevapları kaydedilmiyor *******************************************************************/

    //public async Task<IActionResult> AnketDoldur(int anketId, int sayfa = 1)
    //{
    //    var sorular = await _soruService.GetAllServiceAsync();
    //    // sorular = sorular.Where(x => x.AnketId == anketId).ToList();

    //    if (sorular == null || !sorular.Any())
    //    {
    //        return NotFound("Ankete ait soru bulunamadı.");
    //    }

    //    int toplamSayfa = sorular.Count;
    //    if (sayfa < 1) sayfa = 1;
    //    if (sayfa > toplamSayfa) sayfa = toplamSayfa;

    //    var seciliSoru = sorular[sayfa - 1];

    //    // Session'dan cevaplar alınır
    //    var cevaplar = HttpContext.Session.GetString("Cevaplar");
    //    var cevapSozluk = string.IsNullOrEmpty(cevaplar)
    //        ? new Dictionary<string, string>()
    //        : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

    //    ViewBag.Cevaplar = cevapSozluk;

    //    ViewBag.Sayfa = sayfa;
    //    ViewBag.ToplamSayfa = toplamSayfa;
    //    ViewBag.AnketId = anketId;

    //    return View(seciliSoru);
    //}

    //[HttpPost]
    //public IActionResult AnketDoldur(int anketId, int sayfa, IFormCollection form)
    //{
    //    var cevaplar = HttpContext.Session.GetString("Cevaplar");
    //    var cevapSozluk = string.IsNullOrEmpty(cevaplar)
    //        ? new Dictionary<string, string>()
    //        : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

    //    foreach (var key in form.Keys)
    //    {
    //        if (key.StartsWith("Cevap_"))
    //        {
    //            cevapSozluk[key] = form[key];
    //        }
    //    }

    //    // Session’a tekrar kaydet
    //    HttpContext.Session.SetString("Cevaplar", System.Text.Json.JsonSerializer.Serialize(cevapSozluk));

    //    // Sonraki sayfaya yönlendir
    //    return RedirectToAction("AnketDoldur", new { anketId, sayfa });
    //}

    //sonradan aktifleştirilecek cevapları için
    //[HttpPost]
    //public IActionResult AnketiKaydet(IFormCollection form)
    //{
    //    int anketId = int.Parse(form["AnketId"]);

    //    foreach (var key in form.Keys)
    //    {
    //        if (key.StartsWith("Cevap_"))
    //        {
    //            var soruIdStr = key.Replace("Cevap_", "");
    //            int soruId = int.Parse(soruIdStr);

    //            var cevapDegeri = form[key]; // string veya string[] olabilir

    //            // Burada cevabı veritabanına kaydedebilirsin (Cevap tablosuna)
    //        }
    //    }

    //    TempData["Mesaj"] = "Cevaplar başarıyla kaydedildi.";
    //    return RedirectToAction("Index");
    //}




    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
