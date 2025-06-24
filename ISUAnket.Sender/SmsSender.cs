using ISUAnket.Sender.ViewModel;
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



        public static string smsOlustur(List<LinkViewModel> LinkList)
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

          var paketId=  SendSms(mesajlar, cepTelefonlar);

            return paketId;
        }


        private static string SendSms(List<personalMessage> mesajlar, List<string> cepTelefonlar)
        {
            var paketId = "";

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

                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<SmsResponceViewModel>(resultjson);

                    paketId = result.sendSmsResult.PacketId;
                }
            }
            catch
            {
                paketId = "";
            }

            return paketId;
        }


    }
}
