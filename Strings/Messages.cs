
using System.Xml.Schema;

namespace bmstu_bot.Strings
{
    public static class Messages
    {
        public static string Start = "Привет, Бауманец! Мы очень рады, что ты решил воспользоваться нашим ботом. С его помощью ты можешь задать интересующие тебя вопросы, а также обратиться с проблемой.";
        public static string AskFio = "Скажите своё ФИО";
        public static string FioError = "Неверный ввод\nПопробуйте ещё раз в формате Иван Иванов Иванович.";

        public static string AskGroup = "Ваша учебная группа";

        public static string isAnonimus = "Отправить обращение анонимно?";

        public static string AskCompalin = "Введите текст вашего обращения";
        public static string WaitMsg = "Ваше обращение успешно доставлено администраторам, пожалуйста ожидайте ответа.\nВы можете отправить новое заявление.";
        public static string AskMessageType = "Выберите тип заявления";

        public static string AskComplainCategory = "Выберите категорию обращения";
        public static string redirect_to_VK = "Мы очень рады, что ты интересуешься вопросами поступления и возможно уже твердо решил присоединиться в будущем к большому Бауманскому братству. Свои вопросы по поводу поступления в МГТУ им. Н.Э. Баумана ты можешь задать в личных сообщениях группы для абитуриентов в VK: https://vk.com/ab_bmstu1830\" ";
        public static string NotAllowed = "Только администратор бота может использовать эту команду";
        public static string BadWord = "Пожалуйста, используйте более вежливое обращение";
    }
}
