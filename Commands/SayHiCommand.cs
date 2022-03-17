using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11.Commands
{
    internal class SayHiCommand : AbstractCommand, IChatTextCommand
    {
        public SayHiCommand()
        {
            commandText = "/saymehi";
        }

        string IChatTextCommand.ReturnText()
        {
            return "Привет";
        }
    }
}
