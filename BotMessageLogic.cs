using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace HW11
{
    internal class BotMessageLogic
    {
        private TelegramBotClient _botClient;

        private Dictionary<long, Conversation> _chatList = new Dictionary<long, Conversation>();

        private Messenger _messenger;

        public BotMessageLogic(TelegramBotClient botClient)
        {
            _botClient = botClient;
            _messenger = new Messenger(botClient);
        }

        
        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;

            if (!_chatList.ContainsKey(id))
            {
                var newChat = new Conversation(e.Message.Chat);
                _chatList.Add(id, newChat);
            }

            var chat = _chatList[id];
            chat.AddMessage(e.Message);
            await SendTextMessage(chat);
        }

        private async Task SendTextMessage(Conversation chat)
        {
            _messenger.MakeAnswer(chat);

            
        }
    }
}
