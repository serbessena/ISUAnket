using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ISUAnket.WEB.Controllers
{
    [Authorize(Roles = "SüperAdmin,Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            //personelList();

            //smsGonder();

            //smsGonderimDurum();

            return View();
        }

        private void personelList()
        {
            var Pesonellist = (List<ISUAnket.Service.ViewModels.PersonelSayilariView>)ISUAnket.Service.BYSSQL.SqlSorgu("01.01.02.10.04", 1);
        }

        private void smsGonder()
        {
            var link = new ISUAnket.Sender.ViewModel.LinkViewModel
            {
                Cep = "5366163606",
                Link = "Deneme"
            };

            var paketId = ISUAnket.Sender.SmsSender.smsOlustur(new List<ISUAnket.Sender.ViewModel.LinkViewModel> { link });
        }


        private void smsGonderimDurum()
        {

        }
    }
}
