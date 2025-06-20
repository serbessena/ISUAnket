using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class AnketController : Controller
    {
        private readonly IAnketService _anketService;
        private readonly IKullaniciService _kullaniciService;
        private readonly IDataProtector _protector; //anketId linkini şifreli bir şekilde gönderiri
        private readonly IConfiguration _configuration; 

        public AnketController(IAnketService anketService, IKullaniciService kullaniciService, IDataProtectionProvider provider, IConfiguration configuration)
        {
            _anketService = anketService;
            _kullaniciService = kullaniciService;
            _protector = provider.CreateProtector("AnketIdKoruma"); //anketId değerini şifrelemek icin tanimlandi
            _configuration = configuration;
        }

        public async Task<IActionResult> AnketListesi()
        {
            var sonuc=await _anketService.GetAllServiceAsync();

            return View(sonuc);
        }

        public async Task<IActionResult> AnketDetay(int id)
        {
            var anket=await _anketService.GetByIdServiceAsync(id);

            if (anket==null)
            {
                return NotFound("Anket bulunamadı!");
            }

            return View(anket);
        }

        public async Task<IActionResult> AnketEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnketEkle(Anket model)
        {
            #region Session ile giriş-çıkış işlemleri

            // Oturumdan gelen kullanıcı Id'sini alınır
            //var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            //if (kullaniciId == null)
            //{
            //    return Unauthorized("Kullanıcı oturumu bulunamadı.");
            //}

            #endregion

            // Kullanıcı Id'sini cookie (claims) üzerinden alıyoruz
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (kullaniciIdClaim == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            model.OlusturanKullaniciId = kullaniciId;
            model.AnketDurumu = AnketDurumuEnum.Taslak;
            model.OlusturmaTarihi = DateTime.Now;
            model.AktifMi = true;

            await _anketService.AddServiceAsync(model);

            // Kaydettikten sonra model.Id otomatik oluştu anket oluşur ama AnketId degeri sifresiz olarak goruntulenir
            //var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            //model.Link = $"{siteUrl}/Home/AnketDoldur?anketId={model.Id}";

            string sifreliId = _protector.Protect(model.Id.ToString());

            //localde calisirkenki localhost adresini otomatik olarak alır
            var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;

            
            model.Link = $"{siteUrl}/Home/AnketDoldur?anketId={sifreliId}";


            await _anketService.UpdateServiceAsync(model);

            return RedirectToAction(nameof(AnketListesi));
        }

        public async Task<IActionResult>  AnketDuzenle(int id)
        {
            var anket=await _anketService.GetByIdServiceAsync(id);

            if (anket==null)
            {
                return NotFound("Anket bulunamadı!");
            }

            return View(anket);
        }

        [HttpPost]
        public async Task<IActionResult> AnketDuzenle(Anket model)
        {
            #region Session ile giriş-çıkış işlemleri
            
            //var duzenleyenId = HttpContext.Session.GetInt32("KullaniciId");

            //if (duzenleyenId == null)
            //{
            //    return Unauthorized("Kullanıcı oturumu bulunamadı.");
            //}

            #endregion


            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (kullaniciIdClaim == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            int duzenleyenId = int.Parse(kullaniciIdClaim.Value);


            var mevcutAnket = await _anketService.GetByIdServiceAsync(model.Id);

            if (mevcutAnket == null)
            {
                return NotFound("Anket bulunamadı.");
            }

            mevcutAnket.Ad = model.Ad;
            mevcutAnket.BaslangicTarihi = model.BaslangicTarihi;
            mevcutAnket.BitisTarihi = model.BitisTarihi;
            mevcutAnket.Link=model.Link;
            mevcutAnket.AnketDurumu=model.AnketDurumu;
            mevcutAnket.AktifMi=model.AktifMi;
            //mevcutAnket.DuzenleyenKullaniciId = duzenleyenId.Value; Session ile giriş işlemlerinde aç
            mevcutAnket.DuzenleyenKullaniciId = duzenleyenId;
            mevcutAnket.DuzenlenmeTarihi = DateTime.Now;

            #region Link düzenleme veya olmayan linki oluşturma (AnketId şifresiz bir şekilde tutulur)

            //var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            //mevcutAnket.Link = $"{siteUrl}/Home/AnketDoldur?anketId={mevcutAnket.Id}";

            #endregion

            #region Link düzenleme: AnketId şifrelenmiş olarak eklenecek

            //localdaki siteUrl bilgisini tutar
            //var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            var siteUrl = _configuration["SiteUrl"];
            string sifrelenmisId = _protector.Protect(mevcutAnket.Id.ToString());

            mevcutAnket.Link = $"{siteUrl}/Home/AnketDoldur?anketId={sifrelenmisId}";

            #endregion

            await _anketService.UpdateServiceAsync(mevcutAnket);

            return RedirectToAction(nameof(AnketListesi));
        }

        public async Task<IActionResult> AnketDurumuDegistir(int id)
        {
            await _anketService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(AnketListesi));
        }

        [HttpPost]
        public async Task<IActionResult> AnketSil(int id)
        {
            var anket = await _anketService.GetByIdServiceAsync(id);

            if (anket == null)
            {
                return NotFound("Silinecek anket bulunamadı.");
            }

            await _anketService.DeleteServiceAsync(anket);

            return RedirectToAction(nameof(AnketListesi));
        }
    }
}
