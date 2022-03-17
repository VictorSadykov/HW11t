using HW11.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
namespace HW11
{
    internal class Messenger
    {
        private CommandParser _parser = new CommandParser();
        private TelegramBotClient _botClient;

        public Messenger(TelegramBotClient botClient)
        {
            _botClient = botClient;
            RegisterCommands();
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastMessage = chat.GetLastMessage();

            if (chat.IsTrainingInProcess && !_parser.IsTextCommand(lastMessage))
            {
                _parser.ContinueTraining(lastMessage, chat);
                return;
            }

            if (chat.IsAddingInProcess)
            {
                _parser.NextStage(lastMessage, chat);
                return;
            }

            

            if (_parser.IsMessageCommand(lastMessage))
            {
                await ExecCommand(chat, lastMessage);
            }
            else
                {
                    var text = CreateTextMessage();

                    await SendText(chat, text);
                }
            }


        private async Task ExecCommand(Conversation chat, string lastMessage)
        {
            if (_parser.IsTextCommand(lastMessage))
            {
                var text = _parser.GetMessageText(lastMessage, chat);

                await SendText(chat, text);
            }

            if (_parser.IsButtonCommand(lastMessage))
            {
                var keys = _parser.GetKeyBoard(lastMessage);
                var text = _parser.GetInformationMessage(lastMessage);

                _parser.AddCallback(lastMessage, chat);

                await SendTextWithKeyBoard(chat, text, keys);
            }

            if (_parser.isAddingCommand(lastMessage))
            {
                chat.IsAddingInProcess = true;
                _parser.StartAddingWord(lastMessage, chat);
            }
        }

        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keys)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chat.GetId(),
                text: text,
                replyMarkup: keys
                );

        }

        private string CreateTextMessage()
        {
            return "Not a command";
        }

        private async Task SendText(Conversation chat, string text)
        {
            await _botClient.SendTextMessageAsync(
                chatId: chat.GetId(),
                text: text);
        }


        private void RegisterCommands()
        {
            _parser.AddCommand(new SayHiCommand());
            _parser.AddCommand(new DeleteWordCommand());
            _parser.AddCommand(new StopTrainingCommand());
            _parser.AddCommand(new AddWordCommand(_botClient));
            _parser.AddCommand(new TrainingCommand(_botClient));
        }

    }
}
