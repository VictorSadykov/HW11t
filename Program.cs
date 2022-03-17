using HW11.Commands;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HW11
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            
            var bot = new BotWorker();
            bot.Initialize();
            bot.Start();
            
            Console.ReadKey();

            bot.Stop();
        }

        
    }
    }

