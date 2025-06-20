using ISUAnket.Business.Interfaces;
using ISUAnket.DataAccess.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ISUAnket.WEB.Controllers
{
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IMenuRolService _menuRolService;
        private readonly IRolService _rolService;

        public MenuController(IMenuService menuService, IMenuRolService menuRolService, IRolService rolService)
        {
            _menuService = menuService;
            _menuRolService = menuRolService;
            _rolService = rolService;
        }

        public async Task<IActionResult> MenuListesi()
        {
            var menu=await _menuService.GetListAllServiceAsync();

            return View(menu);
        }

        [HttpGet]
        public IActionResult MenuEkle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MenuEkle(Menu menu)
        {
            await _menuService.AddServiceAsync(menu);

            return RedirectToAction(nameof(MenuListesi));
        }

        [HttpGet]
        public async Task<IActionResult> MenuGuncelle(int id)
        {
            var menu=await _menuService.GetByIdServiceAsync(id);

            if(menu==null)
            {
                return NotFound("Menü bulunamadı!");
            }

            return View(menu);
        }

        [HttpPost]
        public async Task<IActionResult> MenuGuncelle(Menu menu)
        {
            await _menuService.UpdateServiceAsync(menu);

            return RedirectToAction(nameof(MenuListesi));
        }

        public async Task<IActionResult> MenuSil(int id)
        {
            var menu = await _menuService.GetByIdServiceAsync(id);

            if (menu == null)
            {
                return NotFound("Menü bulunamadı.");
            }

            await _menuService.DeleteServiceAsync(menu);

            return RedirectToAction(nameof(MenuListesi));
        }

        public async Task<IActionResult> MenuDurumuDegistir(int id)
        {
            await _menuService.ChangeActivePasiveStatusServiceAsync(id);

            return RedirectToAction(nameof(MenuListesi));
        }
    }
}
