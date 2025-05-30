using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ISUAnket.WEB.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }

        public async Task<IActionResult> RolListesi()
        {
            var sonuc=await _rolService.GetListAllServiceAsync();

            return View(sonuc);
        }

        public IActionResult RolEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RolEkle(Rol model)
        {
            

            await _rolService.AddServiceAsync(model);

            return RedirectToAction(nameof(RolListesi));
        }

        [HttpGet]
        public async Task<IActionResult> RolGuncelle(int id)
        {
            var rol=await _rolService.GetByIdServiceAsync(id);

            if (rol==null)
            {
                return NotFound("Rol bulunamadı!");
            }

            return View(rol);
        }

        [HttpPost]
        public async Task<IActionResult> RolGuncelle(Rol model)
        {

            await _rolService.UpdateServiceAsync(model);

            return RedirectToAction(nameof(RolListesi));
        }

        public async Task<IActionResult> RolSil(int id)
        {
            var rol = await _rolService.GetByIdServiceAsync(id);

            if (rol==null)
            {
                return NotFound("Rol bulunamadı.");
            }

            await _rolService.DeleteServiceAsync(rol);

            return RedirectToAction(nameof(RolListesi));
        }

        public async Task<IActionResult> RolDurumuDegistir(int id)
        {
            await _rolService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(RolListesi));
        }
    }
}
