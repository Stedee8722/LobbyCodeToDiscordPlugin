using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LobbyCodeToDiscord.Utils
{
    internal class DiscordBotUtil
    {
        public static bool sendMessage(string? data, string token, ulong channel_id) {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "LobbyCodeToDiscordMod (v0.0.1)");

            string link = "https://discord.com/api/v10/channels/" + channel_id.ToString() + "/messages";

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
                var read = Task.Run(() => client.PostAsync(link, httpContent));
                read.Wait();
                Console.WriteLine(read.Result);
                return false;
            }
        }

        public static bool editMessage(string? data, string token, ulong channel_id, ulong message_id)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "LobbyCodeToDiscordMod (v0.0.1)");

            string link = "https://discord.com/api/v10/channels/" + channel_id.ToString() + "/messages/" + message_id;

            string payload = "{\"content\": \"" + data + "\"}";
            var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var task = Task.Run(() => client.PatchAsync(link, httpContent));
            task.Wait();
            var response = task.Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var read = Task.Run(() => client.PostAsync(link, httpContent));
                read.Wait();
                Console.WriteLine(read.Result);
                return false;
            }
        }
    }
}
