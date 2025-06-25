using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using ISUAnket.EntityLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class SoruController : Controller
    {
        private readonly ISoruService _soruService;
        private readonly IKullaniciService _kullaniciService;
        private readonly IAnketService _anketService;

        public SoruController(ISoruService soruService, IKullaniciService kullaniciService, IAnketService anketService)
        {
            _soruService = soruService;
            _kullaniciService = kullaniciService;
            _anketService = anketService;
        }

        public async Task<IActionResult> SoruListesi()
        {
            var sonuc = await _soruService.GetAllServiceAsync();

            return View(sonuc);
        }

        public async Task<IActionResult> SoruEkle()
        {
            var anketler=await _anketService.GetListAllServiceAsync();
            ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SoruEkle(Soru model)
        {
            #region Session ile giriş kontrol

            //var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            //if (kullaniciId==null)
            //{
            //    return Unauthorized("Kullanıcı bulunamadı!");
            //}

            #endregion


            // Kullanıcı Id'sini cookie  üzerinden alıyoruz
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (kullaniciIdClaim == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            #region Anket durumuna göre Soru ekleme kontrolü

            if (model.AnketId == null)
            {
                ModelState.AddModelError("", "Lütfen bir anket seçiniz.");
                var anketler = await _anketService.GetListAllServiceAsync();
                ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");
                return View(model);
            }

            var anket = await _anketService.GetByIdServiceAsync(model.AnketId.Value);
            if (anket?.AnketDurumu == AnketDurumuEnum.Yayınlandı)
            {
                ModelState.AddModelError("", "Yayınlanmış ankete soru eklenemez.");
                var anketler = await _anketService.GetListAllServiceAsync();
                ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");
                return View(model);
            }

            #endregion

            if (model.SoruTipi == SoruTipiEnum.TekSatırMetin && model.VeriTipi == null)
            {
                ModelState.AddModelError("VeriTipi", "Lütfen bir veri tipi seçiniz.");
            }


            //model.OlusturanKullaniciId = kullaniciId.Value; //Session ile giriş işlemlerinde kullanılır
            model.OlusturanKullaniciId = kullaniciId;
            model.OlusturmaTarihi = DateTime.Now;
            model.AktifMi = true;

            await _soruService.AddServiceAsync(model);

            return RedirectToAction(nameof(SoruListesi));
        }

        [HttpGet]
        public async Task<IActionResult> SoruGuncelle(int id)
        {
            var soru = await _soruService.GetByIdServiceAsync(id);

            if (soru==null)
            {
                return NotFound("Soru bulunamadı!");
            }

            var anketler=await _anketService.GetListAllServiceAsync();
            ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

            return View(soru);
        }

        [HttpPost]
        public async Task<IActionResult> SoruGuncelle(Soru model)
        {
            #region Session bazlı giriş kontrolü

            //var duzenleyenId = HttpContext.Session.GetInt32("KullaniciId");

            //if (duzenleyenId == null)
            //{
            //    return Unauthorized("Kullanıcı oturumu bulunamadı.");
            //}

            #endregion


            // Cookie (Claims) üzerinden kullanıcı ID'sini al
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (kullaniciIdClaim == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            int duzenleyenId = int.Parse(kullaniciIdClaim.Value);

            var mevcutSoru = await _soruService.GetByIdServiceAsync(model.Id);
            if (mevcutSoru == null)
            {
                return NotFound("Soru bulunamadı.");
            }

            #region VeriTipi kontrolü

            if (model.SoruTipi==SoruTipiEnum.TekSatırMetin && model.VeriTipi==null)
            {
                ModelState.AddModelError("VeriTipi", "Lütfen bir veri tipi seçiniz!");
            }

            #endregion


            #region Anket durumuna göre Soru güncelleme kontrolü

            if (mevcutSoru.AnketId==null)
            {
                ModelState.AddModelError("", "Anket bilgisi bulunamadı!");

                var anketler = await _anketService.GetListAllServiceAsync();
                ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

                return View(model);
            }

            var anket = await _anketService.GetByIdServiceAsync(mevcutSoru.AnketId.Value);

            if (anket?.AnketDurumu==AnketDurumuEnum.Yayınlandı)
            {
                ModelState.AddModelError("", "Yayınlanmış anketteki soru güncellenemez!");

                var anketler = await _anketService.GetListAllServiceAsync();
                ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

                return View(model);
            }

            #endregion

            mevcutSoru.SoruMetni = model.SoruMetni;
            mevcutSoru.AnketId = model.AnketId;
            mevcutSoru.ZorunluMu = model.ZorunluMu;
            //mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value; //Session bazlı giriş için kullanılır
            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId;
            mevcutSoru.DuzenlenmeTarihi = DateTime.Now;

            //mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value; //Session bazlı giriş için kullanılır
            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId;
            mevcutSoru.DuzenlenmeTarihi = DateTime.Now;
            mevcutSoru.SoruTipi = model.SoruTipi;
            mevcutSoru.VeriTipi = model.VeriTipi;
            mevcutSoru.SoruSecenekleri = model.SoruSecenekleri;

            await _soruService.UpdateServiceAsync(mevcutSoru);

            return RedirectToAction(nameof(SoruListesi));
        }

        public async Task<IActionResult> SoruDurumDegistir(int id)
        {
            await _soruService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(SoruListesi));
        }

        [HttpPost]
        public async Task<IActionResult> SoruSil(int id)
        {
            var soru = await _soruService.GetByIdServiceAsync(id);

            if (soru == null)
            {
                return NotFound("Silinecek soru bulunamadı.");
            }

            #region Anket durumuna göre Soru silme kontrolü

            if (soru.AnketId==null)
            {
                TempData["Hata"] = "Ankete ait bilgi bulunamadığı için silme işlemi gerçekleştirilemedi!";

                return RedirectToAction(nameof(SoruListesi));
            }

            var anket=await _anketService.GetByIdServiceAsync(soru.AnketId.Value);

            if (anket?.AnketDurumu==AnketDurumuEnum.Yayınlandı)
            {
                TempData["Hata"] = "Yayınlanmış ankete ait soru silinemez!";

                return RedirectToAction(nameof(SoruListesi));
            }

            #endregion

            await _soruService.DeleteServiceAsync(soru);

            return RedirectToAction(nameof(SoruListesi));
        }
    }
}
