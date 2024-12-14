using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LobbyCodeToDiscord;

namespace LobbyCodeToDiscord.Utils
{
    internal class ConfigUtil
    {
        public static void CreateConfig(string path)
        {
            File.Create(path).Dispose();
            string data = "# Configs for Lobby Code To Discord mod!\n\n" +
                "# Webhook link:\n" +
                "# Can only posts new messages not edit\n" +
                "webhook = https://discord.com/api/webhooks/1234567890123456/abcdefghijklmnopqrstuvwxyz-ABCDEFGHIJKLMNOPQRSTUVWXYZ\n\n" +
                "# Discord Bot:\n" +
                "bot_token = <YOUR_TOKEN_HERE>\n" +
                "channel_id = <CHANNEL_ID>\n" +
                "# If Message ID is blank, it'll send a new message instead of editing\n" +
                "message_id = <MESSAGE_ID>\n\n" +
                "# The message to send, use $lobby_code to replace with the code\n" +
                "message = New lobby code is $lobby_code\n" +
                "# To disable anything please set it to null";
            TextWriter file = new StreamWriter(path);
            file.Write(data);
            file.Close();
        }
    }
}
