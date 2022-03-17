using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace HW11.Commands
{
    internal class TrainingCommand : AbstractCommand, IKeyBoardCommand
    {
        private TelegramBotClient _botClient;

        private Dictionary<long, Conversation> _trainingChats = new Dictionary<long, Conversation>();

        private Dictionary<long, TrainingType> _training = new Dictionary<long, TrainingType>();

        private Dictionary<long, string> _activeWord = new Dictionary<long, string>();


        public TrainingCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            commandText = "/training";
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            List<InlineKeyboardButton> buttonList = new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton
                {
                    Text = "С русского на английский",
                    CallbackData = "rustoeng"
                },

                new InlineKeyboardButton
                {
                    Text = "С английского на русский",
                    CallbackData = "engtorus"
                }
            };

            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        public string InformationalMessage()
        {
            return "Выберите тип тренировки. Для окончания тренировки введите команду /stop";
        }

        public void AddCallBack(Conversation chat)
        {
            _trainingChats.Add(chat.GetId(), chat);
            _botClient.OnCallbackQuery -= Bot_Callback;
            _botClient.OnCallbackQuery += Bot_Callback;
        }

        public async void NextStepAsync(Conversation chat, string message)
        {
            var chatId = chat.GetId();
            var type = _training[chatId];
            var word = _activeWord[chatId];

            var check = chat.CheckWord(type, word, message);

            string text = null;

            if (check)
            {
                text = "Правильно!";
            }
            else
            {
                text = "Неправильно.";
            }

            text += " Следующее слово: ";
            var newWord = chat.GetTrainingWord(type);

            text += newWord;

            _activeWord[chatId] = newWord;

            await _botClient.SendTextMessageAsync(chatId: chatId, text: text);
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            string text = null;
            var id = e.CallbackQuery.Message.Chat.Id;
            var chat = _trainingChats[id];


            switch (e.CallbackQuery.Data)
            {
                case "rustoeng":
                    _training.Add(id, TrainingType.RusToEng);
                    text = chat.GetTrainingWord(TrainingType.RusToEng);

                    break;
                case "engtorus":
                    _training.Add(id, TrainingType.EngToRus);

                    text = chat.GetTrainingWord(TrainingType.EngToRus);
                    break;
                default:
                    break;
            }

            chat.IsTrainingInProcess = true;
            _activeWord.Add(id, text);

            if (_trainingChats.ContainsKey(id))
            {
                _trainingChats.Remove(id);
            }

            await _botClient.SendTextMessageAsync(id, text);
            await _botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }
    }
}
