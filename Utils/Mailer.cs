using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon;
using Amazon.SimpleEmail.Model;
using System.Web;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace semenarna_id2 {
    public class Mailer {

        readonly string senderAddress;
        readonly string receiverAddress;
        readonly string htmlBody;
        public Mailer (string sender, string receive, string html) {
            senderAddress = sender;
            receiverAddress = receive;
            htmlBody = html;
        }

        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public class JsonContent : StringContent {
            public JsonContent(object obj) :
                base(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json") { }
        }


        public void Send() {

            var client = new HttpClient();

            var base64Html = Base64Encode(htmlBody);

            string uri = "http://localhost:3000/send";

            var message = new { from = senderAddress, to = receiverAddress, subject = "random subject", html = base64Html };



            client.PostAsync(uri, new JsonContent(message));

        }
    }
}
