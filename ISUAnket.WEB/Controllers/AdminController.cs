using ISUAnket.Sender.ViewModel;
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

            var paketId = smsGonder();

            var sonuc = smsGonderimDurum(paketId, 3);

            return View();
        }

        private void personelList()
        {
            var Pesonellist = (List<ISUAnket.Service.ViewModels.PersonelSayilariView>)ISUAnket.Service.BYSSQL.SqlSorgu("01.01.02.10.04", 1);
        }

        private string smsGonder()
        {
            var link1 = new ISUAnket.Sender.ViewModel.LinkViewModel
            {
                Cep = "5366163606",
                Link = "Behlül Öztürk"
            };

            var link2 = new ISUAnket.Sender.ViewModel.LinkViewModel
            {
                Cep = "5469547258",
                Link = "Sena Duru"
            };

            var link3 = new ISUAnket.Sender.ViewModel.LinkViewModel
            {
                Cep = "5308217721",
                Link = "Ayça Bayrak"
            };

            var sonuc = ISUAnket.Sender.SmsSender.smsOlustur(new List<ISUAnket.Sender.ViewModel.LinkViewModel> { link1, link2, link3 });

            return sonuc.FirstOrDefault().PacketId;
        }


        private List<smsDurumReturnViewModel> smsGonderimDurum(string paketId, int count)
        {

            var sonuc = ISUAnket.Sender.SmsSender.smsDurumKontrol(paketId, count);

            return sonuc;
        }
    }
}
