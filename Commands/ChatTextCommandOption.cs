using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11.Commands
{
    internal abstract class ChatTextCommandOption : AbstractCommand
    {
        public string CommandText;

        public override bool CheckMessage(string message)
        {
            return message.StartsWith(CommandText);
        }

        public string ClearMessageFromCommand(string message)
        {
            return message.Substring(CommandText.Length + 1).Trim();
        }
    }
}
