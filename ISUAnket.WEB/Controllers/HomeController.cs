﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ISUAnket.WEB.Models;
using ISUAnket.Business.Interfaces;
using System.Text.Json;
using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using Microsoft.AspNetCore.DataProtection;
namespace ISUAnket.WEB.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISoruService _soruService;
    private readonly IBirimService _birimService;
    private readonly ICevapService _cevapService;
    private readonly IAnketService _anketService;
    private readonly IDataProtector _protector;

    public HomeController(ILogger<HomeController> logger, ISoruService soruService, IBirimService birimService, ICevapService cevapService, IAnketService anketService, IDataProtectionProvider provider)
    {
        _logger = logger;
        _soruService = soruService;
        _birimService = birimService;
        _cevapService = cevapService;
        _anketService = anketService;
        _protector = provider.CreateProtector("AnketIdKoruma"); //AnketId değerini sifrelemek icin kullanilir
    }

    public IActionResult Index()
    {
        return View();
    }


    public async Task<IActionResult> BirimSec(string anketId)
    {
        int cozulmusAnketId;
        try
        {
            var cozulmus = _protector.Unprotect(anketId);
            cozulmusAnketId = int.Parse(cozulmus);
        }
        catch
        {
            TempData["Hata"] = "Geçersiz anket bağlantısı.";
            return RedirectToAction("Index");
        }

        var birimler = await _birimService.AktifBirimleriGetirServiceAsync();
        ViewBag.AnketId = anketId; // Şifreli haliyle geri gönderiyoruz
        return View(birimler);
    }

    [HttpPost]
    public async Task<IActionResult> BirimSec(int secilenBirimId, string anketId)
    {
        int cozulmusAnketId;
        try
        {
            var cozulmus = _protector.Unprotect(anketId);
            cozulmusAnketId = int.Parse(cozulmus);
        }
        catch
        {
            TempData["Hata"] = "Geçersiz anket bağlantısı.";
            return RedirectToAction("Index");
        }

        var birim = await _birimService.GetByIdServiceAsync(secilenBirimId);
        if (birim == null)
        {
            TempData["Hata"] = "Lütfen geçerli bir birim seçiniz.";
            return RedirectToAction("BirimSec", new { anketId });
        }

        HttpContext.Session.SetInt32("SecilenBirimId", secilenBirimId);

        return RedirectToAction("Kvkk", new { anketId });
    }

    public async Task<IActionResult> Kvkk(string anketId)
    {
        if (string.IsNullOrEmpty(anketId))
        {
            TempData["Hata"] = "Anket numarası geçerli değil.";
            return RedirectToAction("BirimSec");
        }

        int cozulmusAnketId;
        try
        {
            cozulmusAnketId = Convert.ToInt32(_protector.Unprotect(anketId));
        }
        catch
        {
            TempData["Hata"] = "Anket numarası çözümlenemedi.";
            return RedirectToAction("BirimSec");
        }

        var birimId = HttpContext.Session.GetInt32("SecilenBirimId");
        if (birimId == null)
        {
            return RedirectToAction("BirimSec", new { anketId });
        }

        var birim = await _birimService.GetByIdServiceAsync(birimId.Value);
        if (birim == null)
        {
            TempData["Hata"] = "Birim bulunamadı.";
            return RedirectToAction("BirimSec", new { anketId });
        }

        ViewBag.AnketId = anketId;
        ViewBag.BirimAdi = birim.Ad;

        return View();
    }

    [HttpPost]
    public IActionResult KvkkOnayla(bool KvkkOnay, string anketId)
    {
        if (!KvkkOnay)
        {
            TempData["Hata"] = "Devam etmek için KVKK onay kutusunu işaretlemeniz gerekir.";
            return RedirectToAction("Kvkk", new { anketId });
        }

        HttpContext.Session.SetString("KvkkOnay", "true");

        return RedirectToAction("AnketDoldur", new { anketId, sayfa = 1 });
    }

    public async Task<IActionResult> AnketDoldur(string anketId, int sayfa = 1)
    {
        int cozulmusAnketId;
        try
        {
            var cozulmus = _protector.Unprotect(anketId);
            cozulmusAnketId = int.Parse(cozulmus);
        }
        catch
        {
            return NotFound("Geçersiz anket bağlantısı.");
        }

        // Anket durumu kontrolu
        var anket = await _anketService.GetByIdServiceAsync(cozulmusAnketId);
        if (anket == null)
        {
            return NotFound("Anket bulunamadı.");
        }

        if (anket.AnketDurumu == AnketDurumuEnum.Taslak || anket.AnketDurumu == AnketDurumuEnum.İptal)
        {
            return NotFound("Bu anket şu anda erişime kapalıdır.");
        }

        var simdi = DateTime.Now;
        if (anket.BaslangicTarihi > simdi)
        {
            return NotFound("Bu anket henüz başlamadı.");
        }
        if (anket.BitisTarihi < simdi)
        {
            return NotFound("Bu anketin süresi dolmuştur.");
        }

        var kvkk = HttpContext.Session.GetString("KvkkOnay");
        if (kvkk != "true")
        {
            return RedirectToAction("Kvkk", new { anketId });
        }

        var sorular = await _soruService.GetSorularByAnketIdServiceAsync(cozulmusAnketId);

        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        int toplamSayfa = sorular.Count;
        if (sayfa < 1) sayfa = 1;
        if (sayfa > toplamSayfa) sayfa = toplamSayfa;

        var seciliSoru = sorular[sayfa - 1];

        var cevaplar = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplar)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

        ViewBag.Cevaplar = cevapSozluk;
        ViewBag.Sayfa = sayfa;
        ViewBag.ToplamSayfa = toplamSayfa;
        ViewBag.AnketId = anketId; // Şifreli hali

        return View(seciliSoru);
    }

    [HttpPost]
    public async Task<IActionResult> AnketDoldur(string anketId, int sayfa, IFormCollection form)
    {
        int cozulmusAnketId;
        try
        {
            var cozulmus = _protector.Unprotect(anketId);
            cozulmusAnketId = int.Parse(cozulmus);
        }
        catch
        {
            return NotFound("Geçersiz anket bağlantısı.");
        }

        // Anket durum kontrolü (güvenlik açısından POST'ta da yapalım)
        var anket = await _anketService.GetByIdServiceAsync(cozulmusAnketId);
        if (anket == null)
        {
            return NotFound("Anket bulunamadı.");
        }

        if (anket.AnketDurumu == AnketDurumuEnum.Taslak || anket.AnketDurumu == AnketDurumuEnum.İptal)
        {
            return Unauthorized("Bu anket şu anda erişime kapalıdır.");
        }

        var simdi = DateTime.Now;
        if (anket.BaslangicTarihi > simdi)
        {
            return Unauthorized("Bu anket henüz başlamadı.");
        }
        if (anket.BitisTarihi < simdi)
        {
            return Unauthorized("Bu anketin süresi dolmuştur.");
        }

        // Cevapları Session'dan al
        var cevaplar = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplar)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

        // Anket sorularını getir
        var sorular = await _soruService.GetSorularByAnketIdServiceAsync(cozulmusAnketId);

        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        int toplamSayfa = sorular.Count;
        var seciliSoru = sorular[sayfa - 1]; // Sayfadaki soru adeti

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
        HttpContext.Session.SetString("Cevaplar", JsonSerializer.Serialize(cevapSozluk));

        string islem = form["islem"];

        if (islem == "bitir")
        {
            return RedirectToAction("AnketSonuc", new { anketId });
        }

        return RedirectToAction("AnketDoldur", new { anketId, sayfa = sayfa + 1 });
    }


    //AnketId değeri şifresiz bir şekilde gönderilir
    //public async Task<IActionResult> BirimSec(int anketId)
    //{
    //    var birimler = await _birimService.AktifBirimleriGetirServiceAsync();
    //    ViewBag.AnketId = anketId;
    //    return View(birimler);
    //}

    //[HttpPost]
    //public async Task<IActionResult> BirimSec(int secilenBirimId, int anketId)
    //{
    //    var birim = await _birimService.GetByIdServiceAsync(secilenBirimId);
    //    if (birim == null)
    //    {
    //        TempData["Hata"] = "Lütfen geçerli bir birim seçiniz.";
    //        return RedirectToAction("BirimSec", new { anketId });
    //    }

    //    HttpContext.Session.SetInt32("SecilenBirimId", secilenBirimId);

    //    return RedirectToAction("Kvkk", new { anketId });
    //}

    //AnketId değeri şifresiz bir şekilde gönderilir
    //public async Task<IActionResult> Kvkk(/*int anketId = 1*/ int? anketId)
    //{
    //    if (anketId == null || anketId == 0)
    //    {
    //        TempData["Hata"] = "Anket numarası geçerli değil.";
    //        return RedirectToAction("BirimSec");
    //    }

    //    var birimId = HttpContext.Session.GetInt32("SecilenBirimId");
    //    if (birimId == null)
    //    {
    //        //return RedirectToAction("BirimSec");
    //        return RedirectToAction("BirimSec", new { anketId = anketId });
    //    }

    //    var birim = await _birimService.GetByIdServiceAsync(birimId.Value);

    //    if (birim == null)
    //    {
    //        TempData["Hata"] = "Birim bulunamadı.";
    //        //return RedirectToAction("BirimSec");
    //        return RedirectToAction("BirimSec", new { anketId = anketId });
    //    }

    //    ViewBag.AnketId = anketId;
    //    ViewBag.BirimAdi = birim.Ad;

    //    return View();
    //}

    //[HttpPost]
    //public IActionResult KvkkOnayla(bool KvkkOnay, int anketId)
    //{
    //    if (!KvkkOnay)
    //    {
    //        TempData["Hata"] = "Devam etmek için KVKK onay kutusunu işaretlemeniz gerekir.";
    //        return RedirectToAction("Kvkk", new { anketId });
    //    }

    //    HttpContext.Session.SetString("KvkkOnay", "true");

    //    return RedirectToAction("AnketDoldur", new { anketId = anketId, sayfa = 1 });
    //}

    //AnketId değeri şifresiz bir şekilde gönderiliyor
    //public async Task<IActionResult> AnketDoldur(int anketId, int sayfa = 1)
    //{
    //    // Anket durumu kontrolu
    //    var anket = await _anketService.GetByIdServiceAsync(anketId);
    //    if (anket == null)
    //    {
    //        return NotFound("Anket bulunamadı.");
    //    }

    //    if (anket.AnketDurumu == AnketDurumuEnum.Taslak || anket.AnketDurumu == AnketDurumuEnum.İptal)
    //    {
    //        return NotFound("Bu anket şu anda erişime kapalıdır.");
    //    }

    //    var simdi = DateTime.Now;
    //    if (anket.BaslangicTarihi > simdi)
    //    {
    //        return NotFound("Bu anket henüz başlamadı.");
    //    }
    //    if (anket.BitisTarihi < simdi)
    //    {
    //        return NotFound("Bu anketin süresi dolmuştur.");
    //    }


    //    var kvkk = HttpContext.Session.GetString("KvkkOnay");
    //    if (kvkk != "true")
    //    {
    //        return RedirectToAction("Kvkk", new { anketId });
    //    }

    //    var sorular = await _soruService.GetSorularByAnketIdServiceAsync(anketId);

    //    if (sorular == null || !sorular.Any())
    //        return NotFound("Ankete ait soru bulunamadı.");

    //    int toplamSayfa = sorular.Count;
    //    if (sayfa < 1) sayfa = 1;
    //    if (sayfa > toplamSayfa) sayfa = toplamSayfa;

    //    var seciliSoru = sorular[sayfa - 1];

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
    //public async Task<IActionResult> AnketDoldur(int anketId, int sayfa, IFormCollection form)
    //{
    //    // Anket durum kontrolü (güvenlik açısından POST'ta da yapalım)
    //    var anket = await _anketService.GetByIdServiceAsync(anketId);
    //    if (anket == null)
    //    {
    //        return NotFound("Anket bulunamadı.");
    //    }

    //    if (anket.AnketDurumu == AnketDurumuEnum.Taslak || anket.AnketDurumu == AnketDurumuEnum.İptal)
    //    {
    //        return Unauthorized("Bu anket şu anda erişime kapalıdır.");
    //    }

    //    var simdi = DateTime.Now;
    //    if (anket.BaslangicTarihi > simdi)
    //    {
    //        return Unauthorized("Bu anket henüz başlamadı.");
    //    }
    //    if (anket.BitisTarihi < simdi)
    //    {
    //        return Unauthorized("Bu anketin süresi dolmuştur.");
    //    }


    //    // Cevapları Session'dan al
    //    var cevaplar = HttpContext.Session.GetString("Cevaplar");
    //    var cevapSozluk = string.IsNullOrEmpty(cevaplar)
    //        ? new Dictionary<string, string>()
    //        : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

    //    // Anket sorularını getir
    //    var sorular = await _soruService.GetSorularByAnketIdServiceAsync(anketId);

    //    if (sorular == null || !sorular.Any())
    //        return NotFound("Ankete ait soru bulunamadı.");

    //    int toplamSayfa = sorular.Count;
    //    var seciliSoru = sorular[sayfa - 1]; // Sayfadaki soru adeti

    //    // Cevap formdan alınıyor
    //    string cevapKey = $"Cevap_{seciliSoru.Id}";
    //    string cevapDegeri = form[cevapKey];

    //    // Eğer zorunluysa ve boşsa uyarı ver
    //    if (seciliSoru.ZorunluMu && string.IsNullOrWhiteSpace(cevapDegeri))
    //    {
    //        TempData["Hata"] = "Lütfen bu soruyu cevaplayınız.";
    //        return RedirectToAction("AnketDoldur", new { anketId, sayfa });
    //    }

    //    // Cevabı Session'a ekle
    //    cevapSozluk[cevapKey] = string.Join(";", form[cevapKey]);
    //    HttpContext.Session.SetString("Cevaplar", System.Text.Json.JsonSerializer.Serialize(cevapSozluk));

    //    string islem = form["islem"];

    //    if (islem == "bitir")
    //    {
    //        return RedirectToAction("AnketSonuc", new { anketId });
    //    }

    //    return RedirectToAction("AnketDoldur", new { anketId, sayfa = sayfa + 1 });
    //}

    public async Task<IActionResult> AnketSonuc(int anketId)
    {
        var cevaplarJson = HttpContext.Session.GetString("Cevaplar");
        var cevapSozluk = string.IsNullOrEmpty(cevaplarJson)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplarJson);

        var sorular = await _soruService.GetAllServiceAsync();
        if (sorular == null || !sorular.Any())
            return NotFound("Ankete ait soru bulunamadı.");

        var birimId = HttpContext.Session.GetInt32("SecilenBirimId");
        if (birimId == null)
            return RedirectToAction("BirimSec");

        foreach (var soru in sorular)
        {
            var key = $"Cevap_{soru.Id}";
            if (cevapSozluk.TryGetValue(key, out var verilenCevap))
            {
                var cevap = new Cevap
                {
                    SoruId = soru.Id,
                    VerilenCevap = verilenCevap,
                    CevapTarihi = DateTime.Now,
                    AktifMi = true,
                    BirimId = birimId.Value
                };

                await _cevapService.AddServiceAsync(cevap);
            }
        }

        // Session temizleme
        HttpContext.Session.Remove("Cevaplar");
        HttpContext.Session.Remove("SecilenBirimId");
        HttpContext.Session.Remove("KvkkOnay");

        
        TempData["Bilgi"] = "Anketiniz başarıyla gönderildi!";

        return RedirectToAction("Tesekkurler");
    }

    public IActionResult Tesekkurler()
    {
        return View();
    }

    public IActionResult Hata(int kod)
    {
        switch (kod)
        {
            case 403:
                return View("Error403");
            case 404:
                return View("Error404");
            case 500:
                return View("Error500");
            default:
                return View("Error");
        }

    }

    public IActionResult Yetkisiz()
    {
        return View();
    }

    //public async Task<IActionResult> AnketSonuc(int anketId)
    //{
    //    var cevaplar = HttpContext.Session.GetString("Cevaplar");
    //    var cevapSozluk = string.IsNullOrEmpty(cevaplar)
    //        ? new Dictionary<string, string>()
    //        : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(cevaplar);

    //    var sorular = await _soruService.GetAllServiceAsync();
    //    if (sorular == null || !sorular.Any())
    //        return NotFound("Ankete ait soru bulunamadı.");

    //    var sonucListesi = new List<(string SoruMetni, string Cevap)>();

    //    foreach (var soru in sorular)
    //    {
    //        var key = $"Cevap_{soru.Id}";
    //        if (cevapSozluk.TryGetValue(key, out var cevap) && !string.IsNullOrWhiteSpace(cevap))
    //        {
    //            sonucListesi.Add((soru.SoruMetni, cevap));
    //        }
    //        else
    //        {
    //            string cevapMetni = soru.ZorunluMu ? "Bu soru zorunlu ancak cevap girilmemiş." : "";
    //            sonucListesi.Add((soru.SoruMetni, cevapMetni));
    //        }
    //    }

    //    HttpContext.Session.Remove("Cevaplar");

    //    return View(sonucListesi);
    //}

    

    /**  Anket Soru sayfası sayfalar arası geçiş yapılıyor ama sorunun cevapları kaydedilmiyor ***/

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
