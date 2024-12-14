using Cove.Server.Plugins;
using Cove.Server;
using Cove.Server.Utils;
using LobbyCodeToDiscord.Utils;
using System.Text;

// Change the namespace and class name!
namespace LobbyCodeToDiscord
{
    public class LobbyCodeToDiscord : CovePlugin
    {
        CoveServer Server { get; set; }

        private const string config_name = "discord.cfg";
        private string? Webhook;
        private string? BotToken;
        private ulong? ChannelID;
        private ulong? MessageID;
        private string? Message;

        public LobbyCodeToDiscord(CoveServer server) : base(server) {
            Server = server;
        }

        public override void onInit()
        {
            base.onInit();

            string code = Server.LobbyCode;
            Log("Grabbing config...");
            string filePath = AppDomain.CurrentDomain.BaseDirectory + config_name;
            if (!System.IO.File.Exists(filePath))
            {
                Log("Generating config...");
                ConfigUtil.CreateConfig(filePath);
            }
            Dictionary<string, string> config = ConfigReader.ReadConfig(config_name);
            foreach (string key in config.Keys)
            {
                switch (key)
                {
                    case "webhook":
                        if (config[key] == "https://discord.com/api/webhooks/1234567890123456/abcdefghijklmnopqrstuvwxyz-ABCDEFGHIJKLMNOPQRSTUVWXYZ" || config[key] == "null")
                        {
                            Webhook = null;
                            break;
                        }
                        Webhook = config[key];
                        break;
                    case "bot_token":
                        if (config[key].StartsWith("<") || config[key] == "null")
                        {
                            BotToken = null;
                            break;
                        }
                        BotToken = config[key];
                        break;
                    case "channel_id":
                        if (config[key].StartsWith("<") || config[key] == "null")
                        {
                            ChannelID = null;
                            break;
                        }
                        ChannelID = ulong.Parse(config[key]);
                        break;
                    case "message_id":
                        if (config[key].StartsWith("<") || config[key] == "null")
                        {
                            MessageID = null;
                            break;
                        }
                        MessageID = ulong.Parse(config[key]);
                        break;
                    case "message":
                        Message = process_message(config[key], code);
                        break;
                }
            }

            Log("Parse complete!");
            send();

            RegisterCommand("send_code", (player, args) =>
            {
                if (!IsPlayerAdmin(player)) return;
                if (send()) SendPlayerChatMessage(player, "Sent code successfully!");
                else SendPlayerChatMessage(player, "Nothing sent, check your configs");
            });
            SetCommandDescription("send_code", "Sends lobby code manually");
        }

        public bool send()
        {
            bool flag = false;
            if (Webhook != null)
            {
                if (WebhookUtil.sendToWebhook(Webhook, Message))
                {
                    Log("Sent to webhook!");
                    flag = true;
                }
            }
            if (BotToken != null && ChannelID != null)
            {
                if (MessageID == null)
                {
                    if (DiscordBotUtil.sendMessage(Message, BotToken, (ulong)ChannelID))
                    {
                        Log("Sent to discord bot!");
                        flag = true;
                    }
                }
                else
                {
                    if (DiscordBotUtil.editMessage(Message, BotToken, (ulong)ChannelID, (ulong)MessageID))
                    {
                        Log("Edited discord bot's message!");
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public string process_message(string message, string code)
        {
            return message.Replace("$lobby_code", code);
        }
    }
}