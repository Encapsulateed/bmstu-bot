
namespace bmstu_bot.Types
{
    public static class KeyBoards
    {
        public static InlineKeyboardMarkup startKey = new InlineKeyboardMarkup(new[]
        {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Подать заявление","NEW_USER")
            }
        });
        public static InlineKeyboardMarkup AskMessageTypeKeyBoard = new InlineKeyboardMarkup(new[]
       {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Проблема","SET_MESSAGE_TYPE 0")
            },
              new[]
            {
                InlineKeyboardButton.WithCallbackData("Вопрос","SET_MESSAGE_TYPE 1")
            },
                new[]
            {
                InlineKeyboardButton.WithCallbackData("Предложение","SET_MESSAGE_TYPE 2")
            }

        });
        public static InlineKeyboardMarkup BackToFio = new InlineKeyboardMarkup(new[]
        {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","ASK_FIO")
            }
        });



        public static InlineKeyboardMarkup BackToGroup = new InlineKeyboardMarkup(new[]
        {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","ASK_GROUP")
            }
        });
        public static InlineKeyboardMarkup BackToIsAnon = new InlineKeyboardMarkup(new[]
        {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","ASK_FIO")
            }
        });

        public static InlineKeyboardMarkup anonKeyBoard = new InlineKeyboardMarkup(new[]
        {

            new[]
            {
                InlineKeyboardButton.WithCallbackData("Да","ASK_COMPLAIN")
            },
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Нет","ASK_FIO")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","NEW_USER")
            }
        });
        
    }
}
