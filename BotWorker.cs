using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HW11
{
    internal class BotWorker
    {
        private static TelegramBotClient _botClient;
        private static BotMessageLogic _logic;

        public void Initialize()
        {
            _botClient = new TelegramBotClient(BotCredentials.botToken);
            _logic = new BotMessageLogic(_botClient);
        }

        public void Start()
        {
            _botClient.OnMessage += BotClient_OnMessage;
            _botClient.StartReceiving();
        }

        

        public void Stop()
        {
            _botClient.StopReceiving();
        }

        private async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await _logic.Response(e);
            }
        }
    }
}
