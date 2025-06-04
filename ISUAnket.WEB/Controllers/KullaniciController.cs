using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ISUAnket.WEB.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IRolService _rolService;

        public KullaniciController(IKullaniciService kullaniciService, IRolService rolService)
        {
            _kullaniciService = kullaniciService;
            _rolService = rolService;
        }

        public async Task<IActionResult> KullaniciListesi()
        {
            var sonuc=await _kullaniciService.KullanicilaraGoreRolListesiServiceAsync();

            return View(sonuc);
        }

        public async Task<IActionResult> KullaniciEkle()
        {
            var roller = await _rolService.AktifRolleriGetirServiceAsync();

            ViewBag.RollerListesi = new SelectList(roller, "Id", "RolAdi");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> KullaniciEkle(Kullanici model)
        {
            var sonuc = await _kullaniciService.RegisterAsync(model);

            if (!sonuc)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten kullanılıyor!");
                
                var roller=await _rolService.AktifRolleriGetirServiceAsync();
                ViewBag.RollerListesi = new SelectList(roller, "Id", "RolAdi");

                return View(model);
            }

            return RedirectToAction("KullaniciListesi");
        }

        public async Task<IActionResult> KullaniciGuncelle(int id)
        {
            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(id);

            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı. Lütfen tekrar deneyiniz!");
            }

            var roller = await _rolService.AktifRolleriGetirServiceAsync();

            ViewBag.RollerListesi = new SelectList(roller, "Id", "RolAdi", kullanici.RolId);

            return View(kullanici);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KullaniciGuncelle(Kullanici model)
        {
            //if (!ModelState.IsValid)
            //{
            //    var roller = await _rolService.GetListAllServiceAsync();

            //    ViewBag.RollerListesi = new SelectList(roller, "Id", "RolAdi", model.RolId);

            //    return View(model);
            //}

            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(model.Id);

            if (kullanici==null)
            {
                return NotFound("Kullanıcı bulunamadı. Lütfen tekrar deneyiniz!");
            }

            kullanici.TCKN=model.TCKN;
            kullanici.Ad=model.Ad;
            kullanici.Soyad=model.Soyad;
            kullanici.RolId=model.RolId;

            await _kullaniciService.UpdateServiceAsync(kullanici);

            return RedirectToAction(nameof(KullaniciListesi));
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Kullanici model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var sonuc=await _kullaniciService.RegisterAsync(model);

            if (!sonuc)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten kullanılmaktadır!");

                return View(model);
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string kullaniciAdi,string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir");
            }

            var user = await _kullaniciService.LoginAsync(kullaniciAdi, sifre);

            if (user==null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre!");

                return View();
            }

            //Sesssion işlemleri
            HttpContext.Session.SetInt32("KullaniciId", user.Id);
            HttpContext.Session.SetString("KullaniciAdi", user.KulaniciAdi);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            if (kullaniciId.HasValue)
            {
                await _kullaniciService.LogoutAsync(kullaniciId.Value);
            }

            HttpContext.Session.Clear(); // Oturum bilgileri temizlenir

            return RedirectToAction("Login");
        }

        public IActionResult SifreDegistir()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SifreDegistir(string eskiSifre, string yeniSifre)
        {
            var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            if (kullaniciId == null)
            {
                return RedirectToAction("Login");
            }

            var sonuc = await _kullaniciService.SifreDegistirAsync(kullaniciId.Value, eskiSifre, yeniSifre);

            if (!sonuc)
            {
                ModelState.AddModelError("", "Eski şifre hatalı!");

                return View();
            }

            ViewBag.Mesaj = "Şifrenizi başarıyla değiştirdiniz!";

            
            await _kullaniciService.LogoutAsync(kullaniciId.Value);
            HttpContext.Session.Clear(); // Oturumu temizle

           
            return RedirectToAction("Login");
        }


        #region Profil Bölümü

        public async Task<IActionResult> Profil()
        {
            int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));
            var profil = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);
            return View(profil);
        }

        public async Task<IActionResult> ProfilDuzenle()
        {
            int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));

            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            if (kullanici==null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return View(kullanici);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilDuzenle(Kullanici model)
        {
            int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));
            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            if (kullanici==null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            kullanici.Ad = model.Ad;
            kullanici.Soyad=model.Soyad;
            kullanici.KulaniciAdi = model.KulaniciAdi;
            kullanici.TCKN = model.TCKN;

            await _kullaniciService.UpdateServiceAsync(kullanici);

            return RedirectToAction("Profil");
        }

        #endregion

        public async Task<IActionResult> RolAta(int id)
        {
            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(id);

            if (kullanici == null) 
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            var roller = await _rolService.AktifRolleriGetirServiceAsync();

            ViewBag.RollerListesi = new SelectList(roller, "Id", "RolAdi", kullanici.RolId);

            return View(kullanici);
        }

        [HttpPost]
        public async Task<IActionResult> RolAta(int id,int rolId)
        {
            var sonuc = await _kullaniciService.RolAtaAsync(id, rolId);

            if (!sonuc)
            {
                ModelState.AddModelError("", "Rol atanırken hata oluştu. Lütfen tekrar deneyiniz!");
            }

            return RedirectToAction("KullaniciListesi");
        }

        public async Task<IActionResult> KullaniciDurumuDegistir(int id)
        {
            await _kullaniciService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(KullaniciListesi));
        }
    }
}
