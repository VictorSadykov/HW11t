using System;
using System.Collections.Generic;
using System.Text;

namespace HW11.Commands
{
    internal class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        public StopTrainingCommand()
        {
            commandText = "/stop";
        }

        public bool DoAction(Conversation chat)
        {
            chat.IsTrainingInProcess = false;
            return !chat.IsTrainingInProcess;
        }

        public string ReturnText()
        {
            return "Тренировка остановлена!";
        }
    }
}
