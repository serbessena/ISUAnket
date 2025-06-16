using ISUAnket.DataAccess.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class DashboardController : Controller
    {
        private readonly ISUAnketContext _context;

        public DashboardController(ISUAnketContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //toplam rol sayısı
            var toplamRolSayisi=_context.Roller.Count();
            ViewBag.ToplamRolSayisi=toplamRolSayisi;

            //admin rolü içindeki kullanıcı sayısı
            var adminRol = _context.Roller.FirstOrDefault(x => x.RolAdi == "Admin");
            int adminKullaniciSayisi = 0;

            if (adminRol != null)
            {
                adminKullaniciSayisi = _context.Kullanicilar.Count(x => x.RolId == adminRol.Id);
            }
            
            ViewBag.AdminKullaniciSayisi=adminKullaniciSayisi;

            //süper admin rolü içindeki kullanıcı sayısı
            var superAdminRol = _context.Roller.FirstOrDefault(x => x.RolAdi == "SüperAdmin");
            int superAdminKullaniciSayisi = 0;

            if (superAdminRol != null)
            {
                superAdminKullaniciSayisi = _context.Kullanicilar.Count(x => x.RolId == superAdminRol.Id);
            }

            ViewBag.SuperAdminKullaniciSayisi=superAdminKullaniciSayisi;

            //anket sayısı
            var anketSayisi = _context.Anketler.Count();
            ViewBag.AnketSayisi=anketSayisi;

            //aktif anket sayısı
            var aktifAnketSayisi=_context.Anketler.Where(x=>x.AktifMi==true).Count();
            ViewBag.AktifAnketSayisi=aktifAnketSayisi;
            
            //pasif anket sayısı
            var pasifAnketSayisi=_context.Anketler.Where(x=>x.AktifMi==false).Count();
            ViewBag.PasifAnketSayisi=pasifAnketSayisi;

            //toplam soru sayısı
            var toplamSoruSayisi=_context.Sorular.Count();
            ViewBag.ToplamSoruSayisi=toplamSoruSayisi;

            //aktif soru sayısı
            var aktifSoruSayisi = _context.Sorular.Where(x => x.AktifMi == true).Count();
            ViewBag.AktifSoruSayisi=aktifSoruSayisi;

            //pasif soru sayısı
            var pasifSoruSayisi=_context.Sorular.Where(x=>x.AktifMi==false).Count();
            ViewBag.PasifSoruSayisi=pasifSoruSayisi;

            //kayıtlı kullanıcı sayısı
            var kayitliKullaniciSayisi=_context.Kullanicilar.Count();
            ViewBag.KayitliKullaniciSayisi = kayitliKullaniciSayisi;

            //aktif kullanıcı sayısı
            var aktifKullaniciSayisi = _context.Kullanicilar.Where(x => x.OturumAcikMi == true).Count();
            ViewBag.AktifKullaniciSayisi=aktifKullaniciSayisi;

            return View();
        }
    }
}
