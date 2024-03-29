﻿
namespace bmstu_bot.Bot
{
    internal class UpdatesHandler
    {
        private static void Logger(Update update)
        {
            Console.WriteLine(DateTime.Now.ToShortTimeString());
            Console.WriteLine($"{update.Type}\n");


            if (update.Type == UpdateType.Message)
            {
                if (string.IsNullOrEmpty(update.Message.Text) == false)
                    Console.WriteLine($"Message: {update.Message.Text}\nFrom: {update.Message.From.Id}");



            }
            else
            {
                Console.WriteLine($"Querry: {update.CallbackQuery.Data}\nText: {update.CallbackQuery.Message.Text}\nFrom: {update.CallbackQuery.From.Id}");
            }
            Console.WriteLine();
        }

        public static async Task HandleUpdateAsync(Update update, TelegramBotClient bot)
        {
            if (update != null)
            {
                Logger(update);


                if (update.Type == UpdateType.Message)
                {

                    Message message = update.Message;
                    if (message.Type == MessageType.Text)
                    {
                        await TextHandler(message, bot);
                    }
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    await CallBackHandler(update, bot);
                    await bot.DeleteMessageAsync(update.CallbackQuery.From.Id, update.CallbackQuery.Message.MessageId);

                }


            }
        }
        private static async Task TextHandler(Message message, TelegramBotClient bot)
        {
            long chatId = message.From.Id;
            string message_text = message.Text;

            await bot.SendChatActionAsync(chatId, ChatAction.Typing);


            Message replyMsg = null;
            if (message.ReplyToMessage != null)
                replyMsg = message.ReplyToMessage;
            var link = message.From.Username != null ? "@" + message.From.Username : "";


            await ComandExecutor.Execute(message_text, chatId, bot, link, group_id: message.Chat.Id, reply: replyMsg) ;

        }
        private static async Task CallBackHandler(Update update, TelegramBotClient bot)
        {
            long chatId = update.CallbackQuery.From.Id;
            string query = update.CallbackQuery.Data;

            await bot.SendChatActionAsync(chatId, ChatAction.Typing);

            var link = update.CallbackQuery.From.Username != null ? "@" + update.CallbackQuery.From.Username : "";

            await QueryExecutor.Execute(query, chatId, bot, link);
        }
    }
}
