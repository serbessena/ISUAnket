using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class AnketController : Controller
    {
        private readonly IAnketService _anketService;
        private readonly IKullaniciService _kullaniciService;

        public AnketController(IAnketService anketService, IKullaniciService kullaniciService)
        {
            _anketService = anketService;
            _kullaniciService = kullaniciService;
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

            // Oturumdan gelen kullanıcı Id'sini alınır
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            if (kullaniciId == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            model.OlusturanKullaniciId = kullaniciId.Value;
            model.AnketDurumu = AnketDurumuEnum.Taslak;
            model.OlusturmaTarihi = DateTime.Now;
            model.AktifMi = true;

            await _anketService.AddServiceAsync(model);

            // Kaydettikten sonra model.Id otomatik oluştu
            var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            model.Link = $"{siteUrl}/Home/AnketDoldur?anketId={model.Id}";

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

            var duzenleyenId = HttpContext.Session.GetInt32("KullaniciId");

            if (duzenleyenId == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

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
            mevcutAnket.DuzenleyenKullaniciId = duzenleyenId.Value;
            mevcutAnket.DuzenlenmeTarihi = DateTime.Now;

            #region Link düzenleme veya olmayan linki oluşturma

            var siteUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            mevcutAnket.Link = $"{siteUrl}/Home/AnketDoldur?anketId={mevcutAnket.Id}";

            #endregion


            await _anketService.UpdateServiceAsync(mevcutAnket);

            return RedirectToAction(nameof(AnketListesi));
        }

        public async Task<IActionResult> AnketDurumuDegistir(int id)
        {
            await _anketService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(AnketListesi));
        }
    }
}
