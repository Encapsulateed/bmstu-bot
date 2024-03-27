
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
                InlineKeyboardButton.WithCallbackData("Назад","NEW_USER")
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
        public static InlineKeyboardMarkup complainCategorySelection = new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Учёба","SET_COMPLAIN_CATEGORY 0")
            },
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Общежитие","SET_COMPLAIN_CATEGORY 1")
            }
             ,
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Питание","SET_COMPLAIN_CATEGORY 2")
            }
             ,
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Медицина","SET_COMPLAIN_CATEGORY 3")
             }
             ,
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Военная кафедра","SET_COMPLAIN_CATEGORY 4")
            }
              ,
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Поступление","SET_COMPLAIN_CATEGORY 5")
            },
                
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Документы","SET_COMPLAIN_CATEGORY 6")
            }
             ,  new[]
            {
                InlineKeyboardButton.WithCallbackData("Стипендия и социальные выплаты","SET_COMPLAIN_CATEGORY 7")
            }
            ,  new[]
            {
                InlineKeyboardButton.WithCallbackData("Внеучебная деятельность","SET_COMPLAIN_CATEGORY 8")
            }, 
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Другое","SET_COMPLAIN_CATEGORY 9")
            },
             new[]
            {
                InlineKeyboardButton.WithCallbackData("Назад","NEW_USER")
            }
        });

    }
}
