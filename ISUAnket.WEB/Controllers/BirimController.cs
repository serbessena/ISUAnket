using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class BirimController : Controller
    {
        private readonly IBirimService _birimService;

        public BirimController(IBirimService birimService)
        {
            _birimService = birimService;
        }

        public async Task<IActionResult> BirimListesi()
        {
            var sonuc=await _birimService.GetListAllServiceAsync();

            return View(sonuc);
        }

        public IActionResult BirimEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BirimEkle(Birim model)
        {
            

            await _birimService.AddServiceAsync(model);

            return RedirectToAction(nameof(BirimListesi));
        }

        [HttpGet]
        public async Task<IActionResult> BirimGuncelle(int id)
        {
            var birim=await _birimService.GetByIdServiceAsync(id);

            if (birim==null)
            {
                return NotFound("Birim bulunamadı!");
            }

            return View(birim);
        }

        [HttpPost]
        public async Task<IActionResult> BirimGuncelle(Birim model)
        {

            await _birimService.UpdateServiceAsync(model);

            return RedirectToAction(nameof(BirimListesi));
        }

        public async Task<IActionResult> BirimSil(int id)
        {
            var birim = await _birimService.GetByIdServiceAsync(id);

            if (birim==null)
            {
                return NotFound("Birim bulunamadı.");
            }

            await _birimService.DeleteServiceAsync(birim);

            return RedirectToAction(nameof(BirimListesi));
        }

        public async Task<IActionResult> BirimDurumuDegistir(int id)
        {
            await _birimService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(BirimListesi));
        }
    }
}
