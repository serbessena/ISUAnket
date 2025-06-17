using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
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


            mevcutSoru.SoruMetni = model.SoruMetni;
            mevcutSoru.AnketId = model.AnketId;
            mevcutSoru.ZorunluMu = model.ZorunluMu;
            //mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value; //Session bazlı giriş için kullanılır
            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId;
            mevcutSoru.DuzenlenmeTarihi = DateTime.Now;

            //mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value; //Session bazlı giriş için kullanılır
            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId;
            mevcutSoru.DuzenlenmeTarihi = DateTime.Now;
            mevcutSoru.SoruSecenekleri = model.SoruSecenekleri;

            await _soruService.UpdateServiceAsync(mevcutSoru);

            return RedirectToAction(nameof(SoruListesi));
        }

        public async Task<IActionResult> SoruDurumDegistir(int id)
        {
            await _soruService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(SoruListesi));
        }
    }
}
