using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11
{
    internal class AddingController
    {
        private Dictionary<long, AddingState> ChatAdding = new Dictionary<long, AddingState>();

        public void AddFirstState(Conversation chat)
        {
            ChatAdding.Add(chat.GetId(), AddingState.Russian);
        }

        public void NextStage(string message, Conversation chat)
        {
            var chatId = chat.GetId();
            var currentState = ChatAdding[chatId];
            ChatAdding[chatId] = currentState + 1;

            if (ChatAdding[chatId] == AddingState.Finish)
            {
                chat.IsAddingInProcess = false;
                ChatAdding.Remove(chat.GetId());
            }
        }

        public AddingState GetStage(Conversation chat)
        {
            return ChatAdding[chat.GetId()];
        }
    }
}
