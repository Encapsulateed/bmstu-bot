
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bmstu_bot.Bot
{
    internal class ComandExecutor
    {
        public static async Task Execute(string message, long chatId, TelegramBotClient bot, string link, long group_id, Message reply = null)
        {

            await Task.Run(async () =>
            {
     
                if (reply != null)
                {
                    var admin = new Types.Admin();
                    admin.admin = new Models.Admin() { ChatId = chatId, Link = link };

                    await admin.Get();
                   
                    if (admin.admin is not null )
                    {
                        
                        try
                        {
                            var entry = Entry.GetEntryByMessageIdAndAdmin(group_id, reply.MessageId);
                            
                            var entries = Entry.Get(entry.entry.ComplainId);
                            
                            foreach (var entr in entries)
                            {
                                await bot.DeleteMessageAsync(entr.entry.AdminChat, (int)entr.entry.MessageId);
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
                                    try
                                    {
                                        prev_chat += $"\n=====================\n";

                                        prev_chat += $"*Сообщение*: _{sub_complain.compalin.Message.Replace("🔹Текст обращения:", "")}_\n*Ответ*: {sub_complain.compalin.Answer.Replace("🔹Текст обращения:", "")}";
                                        prev_chat += $"\n=====================\n";

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex);
                                    }
                                }

                            }


                            string generatedResponse = $"Получен ответ на Ваше обращение от *{complain.compalin.Date}*\n{prev_chat}\n_{reply.Text.Split("-----------------")[1]}_\n🔹_Ответ Администратора_:\n_{message}_";


                            InlineKeyboardMarkup btn = new InlineKeyboardMarkup(new[]
                            {
                            new[]
                            {
                                InlineKeyboardButton.WithCallbackData("Продолжить общение",$"CONTINUE {entry.entry.ComplainId}")
                            }
                        }); ;

   

                            complain.compalin.Admin = link;
                            complain.compalin.Answer = message;
                            admin.admin.Link = link;
                            admin.Update();
                            complain.Update();


                            await bot.SendTextMessageAsync(complain.compalin.From, generatedResponse, parseMode: ParseMode.Markdown, replyMarkup: btn);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    else
                    {
                        await bot.SendTextMessageAsync(group_id, Messages.NotAllowed);
                    }
                }


                var user = new Types.User();
                user.user = new Models.User() { ChatId = chatId };
                await user.Get();


                var comand = user.user.ComandLine ?? "";

                if (message == "/start")
                {
                    await bot.SendTextMessageAsync(chatId, Messages.Start, replyMarkup: KeyBoards.startKey);
                }
                else if (message.StartsWith(Environment.GetEnvironmentVariable("ADMIN_ADD")!))
                {

                    long admin_id = long.Parse(message.Split(' ')[2]);

                    var admin = new Types.Admin();
                    admin.admin = new Models.Admin() { ChatId = admin_id };
                    await admin.Add();

                    await bot.SendTextMessageAsync(chatId, "Администратор успешно добавлен");

                }
                else if (message.StartsWith(Environment.GetEnvironmentVariable("ADMIN_DELETE")!))
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
                        string lower_case_msg = string.Empty;
                        List<string> banned_words = new List<string>();
                        try
                        {
                            banned_words = System.IO.File.ReadAllText("..//..//..//Strings//words.txt").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.ToLower()).ToList();
                            lower_case_msg = message.ToLower();
                            Console.WriteLine(banned_words.Count);
                        }
                        catch (Exception file_ex)
                        {
                            Console.WriteLine(file_ex.Message);
                            Console.WriteLine(file_ex);
                        }

                     
                        bool banned = banned_words.Any(word => lower_case_msg.Contains(word));
                        Console.WriteLine(banned);

                        if (banned)
                        {
                            await bot.SendTextMessageAsync(chatId, Messages.BadWord);
                            return;
                        }
                        else
                        {
                            Dictionary<int, string> comp_types = new Dictionary<int, string>() { { 0, "Новая проблема" }, { 1, "Новый вопрос" }, { 2, "Новое предложение" } };
                            Dictionary<int, string> comp_categories = new Dictionary<int, string>() { { 0, "Учёба" }, { 1, "Общежитие" }, { 2, "Питание" },{ 3, "Медицина" }, { 4, "Военная кафедра" }, { 5, "Поступление" },
                                                                                                    {6, "Документы" },{ 7, "Стипендия и социальные выплаты" },{ 8, "Внеучебная деятельность" },{ 9, "Другое" } };

                            Dictionary<int, long> admins_chats = new Dictionary<int, long>() { { 0, Convert.ToInt64(Environment.GetEnvironmentVariable("PROBLEMS_CHAT")!)
                                                                                        }, { 1, Convert.ToInt64(Environment.GetEnvironmentVariable("QUESTIONS_CHAT")!) },
                                                                                           { 2, Convert.ToInt64(Environment.GetEnvironmentVariable("OFFERS_CHAT")!) } };


                            try
                            {


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


                                var complain = new Complain()
                                {
                                    compalin = new Models.Complain()
                                    {
                                        Message = message,
                                        From = chatId,
                                        Date = DateTime.Now,
                                        Type = user.user.ComplainType,
                                        Prev = prev_id,
                                        IsAnon = (bool)user.user.Anonim,
                                        Category = user.user.ComplainCategory
                                    }
                                };
                                complain.compalin.Id = await complain.Add();




                                var backKeyBoard = KeyBoards.BackToGroup;
                                bool isAnon = complain.compalin.IsAnon;



                                await bot.SendTextMessageAsync(chatId, Messages.WaitMsg, replyMarkup: KeyBoards.startKey);

                                string prev_chat = null;
                                string type = "";
                                var chat = await complain.GetChat();

                                if (chat is not null && chat.Count is not 0)
                                {
                                    prev_chat += "Предыдущие сообщения";
                                    foreach (var sub_complain in chat)
                                    {
                                        try
                                        {
                                            prev_chat += $"\n=====================\n";

                                            prev_chat += $"*Сообщение*: _{sub_complain.compalin.Message.Replace("🔹Текст обращения:", "")}_\n*Ответ*: {sub_complain.compalin.Answer.Replace("🔹Текст обращения:", "")}\n*Администратор*: {sub_complain.compalin.Admin}";
                                            prev_chat += $"\n=====================\n";

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex);
                                        }
                                    }

                                    isAnon = chat[0].compalin.IsAnon;
                                }
                                if (isAnon)
                                    backKeyBoard = KeyBoards.BackToIsAnon;


                                string head = $"{comp_types[user.user.ComplainType]}.\n🔹Категория: {comp_categories[user.user.ComplainCategory]}\n🔹Дата отправки: *{complain.compalin.Date}*\n{prev_chat}\n";
                                string text = $"🔹*Текст обращения:*\n_{message}_";

                                if (isAnon is false)
                                {
                                    head = $"{comp_types[user.user.ComplainType]}.\n🔹Категория: {comp_categories[user.user.ComplainCategory]}\n🔹Отправитель: *{user.user.Fio}*\n🔹Учебная группа: *{user.user.BmstuGroup}*\n🔹Дата отправки: *{complain.compalin.Date}*\n{prev_chat}\n";
                                }

                                await complain.Send(bot, head + "-----------------\n" + text, admins_chats[user.user.ComplainType]);

                                user.user.ComandLine = "";
                                user.user.Fio = null;
                                user.user.BmstuGroup = null;
                                if (prev_id == null)
                                {
                                    user.user.Anonim = null;
                                }
                                await user.Update();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }

                       
                    }

                }

            });
        }


    }
}
