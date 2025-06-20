using ISUAnket.Business.Interfaces;
using ISUAnket.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ISUAnket.WEB.Controllers
{
    public class MenuRolController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IMenuRolService _menuRolService;
        private readonly IRolService _rolService;

        public MenuRolController(IMenuService menuService, IMenuRolService menuRolService, IRolService rolService)
        {
            _menuService = menuService;
            _menuRolService = menuRolService;
            _rolService = rolService;
        }


        public async Task<IActionResult> Index()
        {
            var liste=await _menuRolService.GetAllServiceAsync(
                x=>x.Menu !=null && x.Rol!=null,
                x=>x.Menu,
                x=>x.Rol);

            return View(liste);
        }

        public async Task<IActionResult> Ekle()
        {
            ViewBag.Menuler = new SelectList(await _menuService.GetListAllServiceAsync(), "Id", "MenuAdi");
            ViewBag.Roller = new SelectList(await _rolService.GetListAllServiceAsync(), "Id", "RolAdi");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ekle(MenuRol menuRol)
        {
            await _menuRolService.AddServiceAsync(menuRol);

            ViewBag.Menuler = new SelectList(await _menuService.GetListAllServiceAsync(), "Id", "MenuAdi");
            ViewBag.Roller = new SelectList(await _rolService.GetListAllServiceAsync(), "Id", "RolAdi");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Guncelle(int id)
        {
            var entity=await _menuRolService.GetByIdServiceAsync(id);

            if (entity==null)
            {
                return NotFound("Role verilecek menü bulunamadı!");
            }

            ViewBag.Menuler = new SelectList(await _menuService.GetListAllServiceAsync(), "Id", "MenuAdi",entity.MenuId);
            ViewBag.Roller = new SelectList(await _rolService.GetListAllServiceAsync(), "Id", "RolAdi",entity.RolId);

            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Guncelle(MenuRol menuRol)
        {
            await _menuRolService.UpdateServiceAsync(menuRol);

            ViewBag.Menuler = new SelectList(await _menuService.GetListAllServiceAsync(), "Id", "MenuAdi", menuRol.MenuId);
            ViewBag.Roller = new SelectList(await _rolService.GetListAllServiceAsync(), "Id", "RolAdi", menuRol.RolId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sil(int id)
        {
            var model = await _menuRolService.GetByIdServiceAsync(id);

            if (model == null)
            {
                return NotFound("Silinecek kayıt bulunamadı!");
            }

            await _menuRolService.DeleteServiceAsync(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
