using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Sender.ViewModel
{
   public class SmsRequestViewModel
    {
        public int validityPeriod { get; set; }
        public string messageText { get; set; }
        public int accountId { get; set; }
        public string password { get; set; }
        public List<personalMessage> personalMessages { get; set; }
        public bool isCheckBlackList { get; set; }
        public List<string> receiverList { get; set; }
        public bool isEncryptedParameter { get; set; }
        public string originator { get; set; }
        public int userCode { get; set; }
        public string username { get; set; }
    }

    public class SmsResponceViewModel
    {
        public ResultViewModel sendSmsResult { get; set; }
    }
    public class ResultViewModel
    {
        public string ErrorCode { get; set; }
        public string PacketId { get; set; }
        public List<string> MessageIdList { get; set; }
    }


    public class personalMessage
    {
        public List<string> parameter { get; set; }
    }

    public class LinkViewModel
    {
        public string Cep { get; set; }
        public string Link { get; set; }
    }
}
