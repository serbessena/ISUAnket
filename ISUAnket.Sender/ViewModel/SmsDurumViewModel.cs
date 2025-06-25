using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISUAnket.Sender.ViewModel
{
   public class SmsDurumRequestViewModel
    {
        public int accountId { get; set; }
        public string password { get; set; }
        public int userCode { get; set; }
        public string username { get; set; }
        public string packetId { get; set; }

    }

    public class SmsDurumResponceSingleViewModel
    {
        public smsDurumResultSingleViewModel getStatusResult { get; set; }
    }

    public class smsDurumResultSingleViewModel
    {
        public string ErrorCode { get; set; }
        public MessageStatusSingleList MessageStatusList { get; set; }
    }
    public class MessageStatusSingleList
    {
        public MessageStatus MessageStatus { get; set; }
    }

    public class SmsDurumResponceViewModel
    {
        public smsDurumResultViewModel getStatusResult { get; set; }
    }

    public class smsDurumResultViewModel
    {
        public string ErrorCode { get; set; }
        public MessageStatusList MessageStatusList { get; set; }
    }
    public class MessageStatusList
    {
        public List<MessageStatus> MessageStatus { get; set; }
    }

    public class MessageStatus
    {
        public string MessageId { get; set; }
        public string PacketId { get; set; }
        public string Receiver { get; set; }
        public string MessageSize { get; set; }
        public List<string> PersonalParameter { get; set; }
        public string Status { get; set; }
        public string DeliveryDate { get; set; }
        public string Reason { get; set; }
    }

    public class smsDurumReturnViewModel
    {
        public string PacketId { get; set; }
        public string MessageId { get; set; }
        public ISUAnket.EntityLayer.Enums.SmsDurumEnum Status { get; set; }
        public string? Error { get; set; }
    }
}
