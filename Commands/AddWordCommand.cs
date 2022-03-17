using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HW11.Commands
{
    internal class AddWordCommand : AbstractCommand
    {
        private TelegramBotClient _botClient;

        public Dictionary<long, Word> buffer = new Dictionary<long, Word>();

        public AddWordCommand(TelegramBotClient botClient)
        {
            commandText = "/addword";
            _botClient = botClient;
        }

        public async void DoForStageAsync(AddingState addingState, Conversation chat, string message)
        {
            var chatId = chat.GetId();
            var word = buffer[chatId];
            string text = null;

            switch (addingState)
            {
                case AddingState.Russian:
                    word.Russian = message;

                    text = "Введите английское значение слова";
                    break;
                case AddingState.English:
                    word.English = message;

                    text = "Введите тематику";
                    break;
                case AddingState.Theme:
                    word.Theme = message;

                    text = "Успешно! Слово " + word.Russian + " добавлено в словарь.";

                    chat.Dictionary.Add(word.Russian, word);
                    buffer.Remove(chatId);
                    break;
                default:
                    break;
            }

            await SendCommandText(text, chatId);
        }

        public async void StartProcessAsync(Conversation chat)
        {
            var chatId = chat.GetId();

            buffer.Add(chatId, new Word());

            var text = "Введите русское значение слова";

            await SendCommandText(text, chatId);
        }

        private async Task SendCommandText(string text, long chatId)
        {
            await _botClient.SendTextMessageAsync(chatId: chatId, text: text);
        }
    }
}
