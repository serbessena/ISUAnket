using ISUAnket.EntityLayer.Enums;
using ISUAnket.Sender.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ISUAnket.Sender
{
    public class SmsSender
    {
        private static int validityPeriod = 600;
        private static int accountId = 2699;
        private static string password = "yqwyuuqW";
        private static string messageText = "[##content##]";
        private static bool isCheckBlackList = false;
        private static bool isEncryptedParameter = false;
        private static string originator = "ISU";
        private static int userCode = 1309;
        private static string username = "isuabys";



        public static List<smsSendReturnViewModel> smsOlustur(List<LinkViewModel> LinkList)
        {
            var mesajlar = new List<personalMessage>();
            var cepTelefonlar = new List<string>();
            foreach (var link in LinkList)
            {
                var mesaj = new personalMessage();
                mesaj.parameter = new List<string>();
                mesaj.parameter.Add(link.Link);

                cepTelefonlar.Add(link.Cep);
                mesajlar.Add(mesaj);
            }

            var sonuc = SendSms(mesajlar, cepTelefonlar);

            return sonuc;
        }


        public static List<smsDurumReturnViewModel> smsDurumKontrol(string packetId, int count)
        {

            var durum = getSmsDurumKontrol(packetId, count);

            return durum;
        }


        private static List<smsSendReturnViewModel> SendSms(List<personalMessage> mesajlar, List<string> cepTelefonlar)
        {
            var returnSonuc = new List<smsSendReturnViewModel>();

            try
            {
                var sms = new SmsRequestViewModel
                {
                    accountId = accountId,
                    isCheckBlackList = isCheckBlackList,
                    isEncryptedParameter = isEncryptedParameter,
                    messageText = messageText,
                    originator = originator,
                    password = password,
                    personalMessages = mesajlar,
                    receiverList = cepTelefonlar,
                    userCode = userCode,
                    username = username,
                    validityPeriod = validityPeriod
                };


                var jsonMesaj = Newtonsoft.Json.JsonConvert.SerializeObject(sms);

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var mesaj = new HttpRequestMessage(HttpMethod.Post, "https://webservice.asistiletisim.com.tr/rest/api/SmsProxy/sendSms");

                //mesaj.Content = JsonContent.Create(jsonMesaj);

                mesaj.Content = new StringContent(jsonMesaj, Encoding.UTF8, "application/json");

                var sonuc = client.Send(mesaj);

                if (sonuc.IsSuccessStatusCode)
                {
                    using var reader = new StreamReader(sonuc.Content.ReadAsStream());


                    var resultjson = reader.ReadToEnd();
                    resultjson = resultjson.Replace("\\r\\n", "").Replace("\\", "").Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").TrimStart('"').TrimEnd('"');


                    if (mesajlar.Count > 1)
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SmsResponceViewModel>(resultjson);

                        if (result.sendSmsResult.ErrorCode == "0")
                        {

                            foreach (var mesajId in result.sendSmsResult.MessageIdList.MessageId)
                            {
                                returnSonuc.Add(new smsSendReturnViewModel { MessageId = mesajId, PacketId = result.sendSmsResult.PacketId });
                            }
                        }
                    }
                    else
                    {
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SmsResponceSingleViewModel>(resultjson);

                        if (result.sendSmsResult.ErrorCode == "0")
                        {
                            returnSonuc.Add(new smsSendReturnViewModel { MessageId = result.sendSmsResult.MessageIdList.MessageId, PacketId = result.sendSmsResult.PacketId });
                        }
                    }
                }
            }
            catch
            {
                returnSonuc = new List<smsSendReturnViewModel>();
            }

            return returnSonuc;
        }


        private static List<smsDurumReturnViewModel> getSmsDurumKontrol(string packetId, int count)
        {
            var returnSonuc = new List<smsDurumReturnViewModel>();

            try
            {
                var sms = new SmsDurumRequestViewModel
                {
                    accountId = accountId,
                    password = password,
                    userCode = userCode,
                    username = username,
                    packetId = packetId
                };


                var jsonMesaj = Newtonsoft.Json.JsonConvert.SerializeObject(sms);

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var mesaj = new HttpRequestMessage(HttpMethod.Post, "https://webservice.asistiletisim.com.tr/rest/api/SmsProxy/getStatus");

                //mesaj.Content = JsonContent.Create(jsonMesaj);

                mesaj.Content = new StringContent(jsonMesaj, Encoding.UTF8, "application/json");

                var sonuc = client.Send(mesaj);

                if (sonuc.IsSuccessStatusCode)
                {
                    //using var reader = new StreamReader(sonuc.Content.ReadAsStream());

                    var reader = sonuc.Content.ReadAsStringAsync();


                    //var resultjson = reader.ReadToEnd();
                    var resultjson = reader.Result.ToString();
                    resultjson = resultjson.Replace("\\r\\n", "").Replace("\\", "").Replace("\\", "").Replace("\"[", "[").Replace("]\"", "]").TrimStart('"').TrimEnd('"');

                    if (count > 1)
                    {

                        var result = JsonConvert.DeserializeObject<SmsDurumResponceViewModel>(resultjson);

                        foreach (var messageStatus in result.getStatusResult.MessageStatusList.MessageStatus)
                        {
                            returnSonuc.Add(new smsDurumReturnViewModel { Error = messageStatus.Reason, MessageId = messageStatus.MessageId, PacketId = messageStatus.PacketId, Status = messageStatus.Status == "0" ? SmsDurumEnum.Hata : messageStatus.Status == "1" ? SmsDurumEnum.Iletildi : SmsDurumEnum.Iletilmedi });
                        }
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<SmsDurumResponceSingleViewModel>(resultjson);


                        returnSonuc.Add(new smsDurumReturnViewModel { Error = result.getStatusResult.MessageStatusList.MessageStatus.Reason, MessageId = result.getStatusResult.MessageStatusList.MessageStatus.MessageId, PacketId = result.getStatusResult.MessageStatusList.MessageStatus.PacketId, Status = result.getStatusResult.MessageStatusList.MessageStatus.Status == "0" ? SmsDurumEnum.Hata : result.getStatusResult.MessageStatusList.MessageStatus.Status == "1" ? SmsDurumEnum.Iletildi : SmsDurumEnum.Iletilmedi });

                    }
                }
            }
            catch
            {
                returnSonuc = new List<smsDurumReturnViewModel>();
            }

            return returnSonuc;
        }


    }
}
