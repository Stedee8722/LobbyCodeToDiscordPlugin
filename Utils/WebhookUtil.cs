using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace LobbyCodeToDiscord.Utils
{
    internal class WebhookUtil
    {
        public static bool sendToWebhook(string link, string? data) {
            HttpClient client = new HttpClient();

            string payload = "{\"content\": \"" + data + "\"}";
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var task = Task.Run(() => client.PostAsync(link, httpContent));
            task.Wait();
            var response = task.Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
