global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using Telegram.Bot.Types.Enums;
global using Telegram.Bot;
global using Telegram.Bot.Types;
global using Telegram.Bot.Polling;
global using System.Data;
global using Microsoft.EntityFrameworkCore;
global using Telegram.Bot.Types.ReplyMarkups;
global using bmstu_bot.Bot;
global using bmstu_bot.Strings;
global using bmstu_bot.Types;
global using System.Text.RegularExpressions;

try
{

    await Bot.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"Exeption in Program.cs\n Function: Bot.Start()\n\n {ex}\n\n");
}