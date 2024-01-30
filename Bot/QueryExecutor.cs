﻿
namespace bmstu_bot.Bot
{
    internal class QueryExecutor
    {
        public static async Task Execute(string query, long chatId, TelegramBotClient bot,string link)
        {
            await Task.Run(async () =>
            {
                var user = new Types.User();
                user.user = new Models.User() { ChatId = chatId, TgLink = link };

                await user.Get();

                if (query == "NEW_USER")
                {
                    await user.Add();


                    await bot.SendTextMessageAsync(chatId, Messages.AskMessageType, replyMarkup: KeyBoards.AskMessageTypeKeyBoard);


                }
                if (query.StartsWith("SET_MESSAGE_TYPE"))
                {
                    await bot.SendTextMessageAsync(chatId, Messages.isAnonimus, replyMarkup: KeyBoards.anonKeyBoard);


                    int type = int.Parse(query.Split(' ')[1]);
                    user.user.ComplainType = type;
                    await user.Update();


                }
                if (query == "ASK_FIO")
                {
                    await bot.SendTextMessageAsync(chatId, Messages.AskFio);
                    user.user.ComandLine = "ASK_FIO";
                    await user.Update();
                }
                if (query.StartsWith("CONTINUE"))
                {
                    int prev_id = int.Parse(query.Split(' ')[1]);

                    user.user.ComandLine = $"CONTINUE_COMPLAIN {prev_id}";

                    bool isAnon = user.user.Anonim ?? true;

                    var backKeyBoard = KeyBoards.BackToGroup;

                    if (isAnon)
                        backKeyBoard = KeyBoards.BackToIsAnon;

                    await bot.SendTextMessageAsync(chatId, Messages.AskCompalin, replyMarkup: backKeyBoard);

                    await user.Update();
                }
                else if(query == "ASK_GROUP")
                {
                    await bot.SendTextMessageAsync(chatId, Messages.AskGroup, replyMarkup: KeyBoards.BackToFio);

                    user.user.ComandLine = "ASK_GROUP";



                    await user.Update();
                }
                else if(query == "ASK_COMPLAIN")
                {
                    user.user.ComandLine = "ASK_COMPLAIN";

                    bool isAnon = user.user.Anonim ?? true;

                    var backKeyBoard = KeyBoards.BackToGroup;

                    if (isAnon)
                        backKeyBoard = KeyBoards.BackToIsAnon;

                    await bot.SendTextMessageAsync(chatId, Messages.AskCompalin, replyMarkup: backKeyBoard);

                    await user.Update();
                }
              
            });
        }
    }
}