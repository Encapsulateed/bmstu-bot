
namespace bmstu_bot.Bot
{
    internal class ComandExecutor
    {
        public static async Task Execute(string message, long chatId, TelegramBotClient bot, string link, Message reply = null)
        {


            await Task.Run(async () =>
            {
                if (reply != null)
                {
                    var admin = new Types.Admin();
                    admin.admin = new Models.Admin() { ChatId = chatId, Link = link };

                    await admin.Get();

                    if (admin is not null)
                    {
                        var entry = Entry.GetEntryByMessageIdAndAdmin(chatId, reply.MessageId);
                        var entries = Entry.Get(entry.entry.ComplainId);

                        foreach (var entr in entries)
                        {
                            if (entr.entry.AdminChat != chatId)
                            {
                                await bot.DeleteMessageAsync(entr.entry.AdminChat, (int)entr.entry.MessageId);
                            }
                        }
                        var complain = new Complain() { compalin = new Models.Complain() { Id = entry.entry.ComplainId } };

                        await complain.Get();

                        string prev_chat = null;
                        var chat = await complain.GetChat();

                        if (chat is not null && chat.Count is not 0)
                        {
                            prev_chat += "Предыдущие сообщения";
                            foreach (var sub_complain in chat)
                            {
                                prev_chat += $"\n=====================\n";
                                prev_chat += $"*Сообщение*: _{sub_complain.compalin.Message.Replace("🔹Текст обращения:", "")}_\n*Ответ*: {sub_complain.compalin.Answer.Replace("🔹Текст обращения:", "")}";
                                prev_chat += $"\n=====================\n";
                            }
                        }





                        string generatedResponse = $"Получен ответ на Ваше обращение от *{complain.compalin.Date}*\n_{reply.Text.Split("-----------------")[1]}_\n{prev_chat}\n🔹_Ответ Администратора_:\n_{message}_";


                        InlineKeyboardMarkup btn = new InlineKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Продолжить общение",$"CONTINUE {entry.entry.ComplainId}")
                            }
                        }); ;

                        await bot.SendTextMessageAsync(complain.compalin.From, generatedResponse, parseMode: ParseMode.Markdown, replyMarkup: btn);

                        await bot.DeleteMessageAsync(chatId, (int)entry.entry.MessageId);

                        complain.compalin.Admin = link;
                        complain.compalin.Answer = message;
                        admin.admin.Link = link;
                        admin.Update();
                        complain.Update();
                    }
                }


                var user = new Types.User();
                Console.WriteLine(chatId);
                user.user = new Models.User() { ChatId = chatId };
                await user.Get();

                Console.WriteLine(user.user.ChatId);

                var comand = user.user.ComandLine ?? "";

                if (message == "/start")
                {
                    await bot.SendTextMessageAsync(chatId, Messages.Start, replyMarkup: KeyBoards.startKey);
                }
                else if (message.StartsWith(Tokens.AdminCreation))
                {

                    long admin_id = long.Parse(message.Split(' ')[2]);

                    var admin = new Types.Admin();
                    admin.admin = new Models.Admin() { ChatId = admin_id };
                    await admin.Add();

                    await bot.SendTextMessageAsync(chatId, "Администратор успешно добавлен");

                }
                else if (message.StartsWith(Tokens.AdminDelete))
                {
                    long admin_id = long.Parse(message.Split(' ')[2]);

                    var admin = new Types.Admin();
                    admin.admin = new Models.Admin() { ChatId = admin_id };
                    await admin.Remove();

                    await bot.SendTextMessageAsync(chatId, "Администратор успешно удалён");
                }
                else if (message is not "/start")
                {

                    if (comand == "ASK_FIO")
                    {
                        if (Regex.IsMatch(message, @"^(\w+\s+){1,3}\w+$"))
                        {
                            user.user.Fio = message;
                            user.user.Anonim = false;

                            user.user.ComandLine = "ASK_GROUP";

                            await bot.SendTextMessageAsync(chatId, Messages.AskGroup, replyMarkup: KeyBoards.BackToFio);


                            await user.Update();
                        }
                        else
                        {
                            await bot.SendTextMessageAsync(chatId, Messages.FioError);
                        }
                    }
                    else if (comand == "ASK_GROUP")
                    {
                        user.user.BmstuGroup = message;
                        user.user.ComandLine = "ASK_COMPLAIN";

                        await bot.SendTextMessageAsync(chatId, Messages.AskCompalin, replyMarkup: KeyBoards.BackToGroup);


                        await user.Update();
                    }
                    else if (comand == "ASK_COMPLAIN" || comand.StartsWith("CONTINUE_COMPLAIN"))
                    {

                        Dictionary<int, string> comp_types = new Dictionary<int, string>() { { 0,"Новая проблема" },{ 1, "Новый вопрос" }, { 2, "Новое предложение" } };
                        int? prev_id = null;
                        try
                        {
                            prev_id = int.Parse(comand.Split(' ')[1]);

                        }
                        catch (Exception)
                        {
                        }

                        if (user.user.Anonim == null)
                            user.user.Anonim = true;


                        var complain = new Complain() { compalin = new Models.Complain() { Message = message, From = chatId, Date = DateTime.Now, Type = user.user.ComplainType, Prev = prev_id,IsAnon = (bool)user.user.Anonim} };
                        complain.compalin.Id = await complain.Add();


                        

                        var backKeyBoard = KeyBoards.BackToGroup;
                        bool isAnon = complain.compalin.IsAnon;
               


                        await bot.SendTextMessageAsync(chatId, Messages.WaitMsg, replyMarkup: KeyBoards.startKey);


                        string prev_chat = null;
                        string type = "";
                        var chat = await complain.GetChat();
                        Console.WriteLine(chat == null);
                        Console.WriteLine(chat.Count);
                        Console.WriteLine(complain.compalin.IsAnon);
                        if (chat is not null && chat.Count is not 0)
                        {
                            prev_chat += "Предыдущие сообщения";
                            foreach (var sub_complain in chat)
                            {
                                prev_chat += $"\n=====================\n";
                                prev_chat += $"*Сообщение*: _{sub_complain.compalin.Message.Replace("🔹Текст обращения:", "")}_\n*Ответ*: {sub_complain.compalin.Answer.Replace("🔹Текст обращения:", "")}\n*Администратор*: {sub_complain.compalin.Admin}";
                                prev_chat += $"\n=====================\n";


                            }

                            isAnon = chat[0].compalin.IsAnon;
                        }
                        Console.WriteLine(complain.compalin.IsAnon);
                        if (isAnon)
                            backKeyBoard = KeyBoards.BackToIsAnon;


                        string head = $"{comp_types[user.user.ComplainType]}.\n🔹Дата отправки: *{complain.compalin.Date}*\n{prev_chat}\n";
                        string text = $"🔹*Текст обращения:*\n_{message}_";

                        if (isAnon is false)
                        {
                            head = $"{comp_types[user.user.ComplainType]}.\n🔹Отправитель: *{user.user.Fio}*\n🔹Учебная группа: *{user.user.BmstuGroup}*\n🔹Дата отправки: *{complain.compalin.Date}*\n{prev_chat}\n";
                        }

                        await complain.Send(bot, head + "-----------------\n" + text);

                        user.user.ComandLine = "";
                        if(prev_id == null)
                        {
                            user.user.Anonim = null;
                        }
                        await user.Update();


                    }

                }

            });
        }


    }
}
