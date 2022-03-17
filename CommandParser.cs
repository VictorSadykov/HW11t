using HW11.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace HW11
{
    internal class CommandParser
    {
        private List<IChatCommand> _commands = new List<IChatCommand>();

        private AddingController _addingController = new AddingController();


        public bool isAddingCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));

            return command is AddWordCommand;
        }

        public void StartAddingWord(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as AddWordCommand;

            _addingController.AddFirstState(chat);
            command.StartProcessAsync(chat);
        }

        public void NextStage(string message, Conversation chat)
        {
            var command = _commands.Find(x => x is AddWordCommand) as AddWordCommand;

            command.DoForStageAsync(_addingController.GetStage(chat), chat, message);

            _addingController.NextStage(message, chat);
        }

        public bool IsTextCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));
            return command is IChatTextCommand;
        }

        public bool IsButtonCommand(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        public bool IsMessageCommand(string message)
        {
            return _commands.Exists(x => x.CheckMessage(message));
        }

        public string GetMessageText(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            if (command is IChatTextCommandWithAction)
            {
                if (!(command as IChatTextCommandWithAction).DoAction(chat))
                {
                    return "Ошибка выполнения.";
                }
            }

            return command.ReturnText();
        }

        public string GetInformationMessage(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            return command.InformationalMessage();
        }

        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            return command.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = _commands.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command.AddCallBack(chat);
        }

        public void ContinueTraining(string message, Conversation chat)
        {
            var command = _commands.Find(x => x is TrainingCommand) as TrainingCommand;
            command.NextStepAsync(chat, message);
        }

        internal void AddCommand(IChatCommand command)
        {
            _commands.Add(command);
        }
    }
}
