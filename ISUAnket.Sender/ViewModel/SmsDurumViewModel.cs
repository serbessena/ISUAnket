using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Sender.ViewModel
{
   public class SmsDurumViewModel
    {
        public int accountId { get; set; }
        public string password { get; set; }
        public int userCode { get; set; }
        public string username { get; set; }
        public string packetId { get; set; }

    }
}
