using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace HW11
{
    internal class Conversation
    {
        private Chat _telegramChat;

        private List<Message> _telegramMessages = new List<Message>();

        public Dictionary<string, Word> Dictionary = new Dictionary<string, Word>();

        public bool IsAddingInProcess;

        public bool IsTrainingInProcess;

        public Conversation(Chat chat)
        {
            _telegramChat = chat;
        }

        public void ClearHistory()
        {
            _telegramMessages.Clear();
        }

        public void AddMessage(Message message)
        {
            _telegramMessages.Add(message);
        }

        public void AddWord(string key, Word word)
        {
            Dictionary.Add(key, word);
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in _telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public string GetFirstName() => _telegramChat.FirstName;

        public long GetId() => _telegramChat.Id;

        public string GetLastMessage() => _telegramMessages.Last().Text;

        internal string GetTrainingWord(TrainingType type)
        {
            var rnd = new Random();
            var item = rnd.Next(0, Dictionary.Count);

            var randomword = Dictionary.Values.AsEnumerable().ElementAt(item);

            var text = string.Empty;

            switch (type)
            {
                case TrainingType.RusToEng:
                    text = randomword.English;
                    break;
                case TrainingType.EngToRus:
                    text = randomword.Russian;
                    break;
                default:
                    break;
            }

            return text;
        }

        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;

            var result = false;

            switch (type)
            {
                case TrainingType.RusToEng:
                    control = Dictionary.Values.FirstOrDefault(x => x.English == word);

                    result = control.Russian == answer;
                    break;
                case TrainingType.EngToRus:
                    control = Dictionary[word];

                    result = control.English == answer;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
