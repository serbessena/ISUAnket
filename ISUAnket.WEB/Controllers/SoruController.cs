using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISUAnket.WEB.Controllers
{
    //[Authorize(Roles = "SüperAdmin,Admin")]
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
            //if(!ModelState.IsValid)
            //{
            //    var anketler = await _anketService.GetListAllServiceAsync();
            //    ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

            //    return View(model);
            //}

            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");
            if (kullaniciId==null)
            {
                return Unauthorized("Kullanıcı bulunamadı!");
            }

            model.OlusturanKullaniciId = kullaniciId.Value;
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
            //if (!ModelState.IsValid) 
            //{
            //    var anketler = await _anketService.GetListAllServiceAsync();
            //    ViewBag.AnketListesi = new SelectList(anketler, "Id", "Ad");

            //    return View(model);
            //}

            var duzenleyenId = HttpContext.Session.GetInt32("KullaniciId");

            if (duzenleyenId == null)
            {
                return Unauthorized("Kullanıcı oturumu bulunamadı.");
            }

            var mevcutSoru = await _soruService.GetByIdServiceAsync(model.Id);
            if (mevcutSoru == null)
            {
                return NotFound("Soru bulunamadı.");
            }


            mevcutSoru.SoruMetni = model.SoruMetni;
            mevcutSoru.AnketId = model.AnketId;
            mevcutSoru.ZorunluMu = model.ZorunluMu;
            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value;
            mevcutSoru.DuzenlenmeTarihi = DateTime.Now;

            mevcutSoru.DuzenleyenKullaniciId = duzenleyenId.Value;
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
