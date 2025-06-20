using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
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
            kullanici.Email=model.Email;
            kullanici.RolId=model.RolId;

            // Eğer kullanıcı şifreyi değiştirmemişse, dummy ise eski şifreyi koru
            if (!string.IsNullOrWhiteSpace(model.Sifre) && model.Sifre != "dummy-password")
            {
                kullanici.Sifre = model.Sifre; // burada hashlenmiş olmalı
            }

            await _kullaniciService.UpdateServiceAsync(kullanici);

            return RedirectToAction(nameof(KullaniciListesi));
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(string kullaniciAdi,string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            {
                ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir");
                return View();
            }

            var user = await _kullaniciService.LoginAsync(kullaniciAdi, sifre);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre!");
                return View();
            }

            // Kullanıcı giriş yapınca cookie içine claim'leri koy
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.KulaniciAdi),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Rol?.RolAdi ?? "User"),// Rol yoksa User ata
                new Claim("AdSoyad", $"{user.Ad} {user.Soyad}")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // kalıcı cookie (remember me gibi)
                ExpiresUtc = DateTime.UtcNow.AddHours(12)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Index", "Admin");

            #region Session ile giriş işlemleri için kullanılır

            //if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(sifre))
            //{
            //    ModelState.AddModelError("", "Kullanıcı adı ve şifre gereklidir");
            //}

            //var user = await _kullaniciService.LoginAsync(kullaniciAdi, sifre);

            //if (user==null)
            //{
            //    ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre!");

            //    return View();
            //}


            ////Sesssion işlemleri
            //HttpContext.Session.SetInt32("KullaniciId", user.Id);
            //HttpContext.Session.SetString("KullaniciAdi", user.KulaniciAdi);
            //HttpContext.Session.SetString("KullaniciRolu", user.Rol?.RolAdi ?? ""); // Rol adı ekle

            //return RedirectToAction("Index", "Home");

            #endregion


        }

        public async Task<IActionResult> Logout()
        {
            // Cookie'deki kullaniciId'yi çekiyoruz (claimlerden)
            var kullaniciIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(kullaniciIdStr, out int kullaniciId))
            {
                // Veritabanını güncelle (Senin mevcut kodun çalışıyor)
                await _kullaniciService.LogoutAsync(kullaniciId);
            }

            // Cookie'yi temizle (Authentication'dan çıkış yap)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");

            #region Session giriş işlemleri için kullanılır

            //var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            //if (kullaniciId.HasValue)
            //{
            //    await _kullaniciService.LogoutAsync(kullaniciId.Value);
            //}

            //HttpContext.Session.Clear(); // Oturum bilgileri temizlenir

            //return RedirectToAction("Login");

            #endregion
            

        }

        public IActionResult SifreDegistir()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SifreDegistir(string eskiSifre, string yeniSifre)
        {

            // Session yerine cookie claims üzerinden kullanıcıyı al:
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null)
            {
                return RedirectToAction("Login");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            var sonuc = await _kullaniciService.SifreDegistirAsync(kullaniciId, eskiSifre, yeniSifre);

            if (!sonuc)
            {
                ModelState.AddModelError("", "Eski şifre hatalı!");
                return View();
            }

            ViewBag.Mesaj = "Şifrenizi başarıyla değiştirdiniz!";

            // Otomatik çıkış yapalım:
            await _kullaniciService.LogoutAsync(kullaniciId);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  // Cookie silinir

            return RedirectToAction("Login");

            #region Session bazlı giriş çıkış kontrolü

            //var kullaniciId = HttpContext.Session.GetInt32("KullaniciId");

            //if (kullaniciId == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //var sonuc = await _kullaniciService.SifreDegistirAsync(kullaniciId.Value, eskiSifre, yeniSifre);

            //if (!sonuc)
            //{
            //    ModelState.AddModelError("", "Eski şifre hatalı!");

            //    return View();
            //}

            //ViewBag.Mesaj = "Şifrenizi başarıyla değiştirdiniz!";


            //await _kullaniciService.LogoutAsync(kullaniciId.Value);
            //HttpContext.Session.Clear(); // Oturumu temizle


            //return RedirectToAction("Login");

            #endregion
        }


        #region Profil Bölümü

        public async Task<IActionResult> Profil()
        {
            // Cookie içerisindeki NameIdentifier claim'inden kullanıcı ID'sini al
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null)
            {
                // Kullanıcı oturum açmamışsa login sayfasına yönlendir
                return RedirectToAction("Login", "Kullanici");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            var profil = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);
            return View(profil);

            #region Session bazlı giriş çıkış kontrolü

            //int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));
            //var profil = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            //return View(profil);

            #endregion

        }

        public async Task<IActionResult> ProfilDuzenle()
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            return View(kullanici);

            #region Session bazlı giriş kontrol işlemleri

            //int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));

            //var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            //if (kullanici==null)
            //{
            //    return NotFound("Kullanıcı bulunamadı!");
            //}

            //return View(kullanici);

            #endregion

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilDuzenle(Kullanici model)
        {
            var kullaniciIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (kullaniciIdClaim == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            int kullaniciId = int.Parse(kullaniciIdClaim.Value);

            var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            if (kullanici == null)
            {
                return NotFound("Kullanıcı bulunamadı!");
            }

            kullanici.TCKN = model.TCKN;
            kullanici.Ad = model.Ad;
            kullanici.Soyad = model.Soyad;
            kullanici.KulaniciAdi = model.KulaniciAdi;
            kullanici.Email = model.Email;

            await _kullaniciService.UpdateServiceAsync(kullanici);

            return RedirectToAction("Profil");


            #region Session bazlı giriş işlemlerinde kullanılır

            //int kullaniciId = Convert.ToInt32(HttpContext.Session.GetInt32("KullaniciId"));
            //var kullanici = await _kullaniciService.KullaniciProfilGetirAsync(kullaniciId);

            //if (kullanici==null)
            //{
            //    return NotFound("Kullanıcı bulunamadı!");
            //}

            //kullanici.TCKN = model.TCKN;
            //kullanici.Ad = model.Ad;
            //kullanici.Soyad=model.Soyad;
            //kullanici.KulaniciAdi = model.KulaniciAdi;
            //kullanici.Email = model.Email;

            //await _kullaniciService.UpdateServiceAsync(kullanici);

            //return RedirectToAction("Profil");

            #endregion

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
