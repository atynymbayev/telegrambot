using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot
{
    public static class Program
    {
        static TelegramBotClient Bot = new TelegramBotClient("5211275691:AAFaDwcbhhAi2wfp_Pry_xBxDg-vpNi6huE");
        public static string General_language;
        public static string answer_language;
        public static string my_question;
        public static string submenu_name;
        public static string submenu_answer_lang;
        public static string subfaq;
        public static string subfaq_answer;
        public static string search;
        public static string fio = null;
        public static string phone = null;
        public static string your_question = null;
        public static string search_text = null;
        public static int step;
        public static bool search_bool;
        public static Dictionary<int, string> langchat = new Dictionary<int, string>();
        static void Main(string[] args)
        {
            Bot.StartReceiving();
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Console.ReadLine();

        }
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {

            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            if (message.Text.Split(' ').First() == "/start")
            {
                await SendInlineKeyboard(message);
            }
            else if (message.Text != null)
            {
                if (step == 1)
                {
                    fio = message.Text;
                    step++;
                    await FIO(message);
                }
                else if (step == 2)
                {
                    phone = message.Text;
                    step++;
                    await Phone(message);
                }
                else if (step == 3)
                {
                    your_question = message.Text;
                    step++;
                    await Question(message);
                }
                else if (search_bool)
                {
                    search_text = message.Text;
                    search_bool = false;
                    await SendKeyboard(message);
                }

            }
        }
        static async Task SendInlineKeyboard(Message message)
        {
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            int id=0;
            // Simulate longer running task
            await Task.Delay(500);

            var inlineKeyboard = new InlineKeyboardMarkup(new[]
            {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("KZ", "KZ"),
                        InlineKeyboardButton.WithCallbackData("RU", "RU"),
                        InlineKeyboardButton.WithCallbackData("EN", "EN"),
                    }
                });

            await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "Тілді таңдаңыз/Выберите язык/Choose language", replyMarkup: inlineKeyboard);

        }

      
        static async Task SendKeyboard(Message message)
        {
            var list_faq = SubFAQ.GetAnswerBySearch(subfaq, search_text);
            var twoMenu = new List<InlineKeyboardButton[]>();
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            foreach (var b in list_faq)
            {
                buttons.Add(InlineKeyboardButton.WithCallbackData(b.SubFAQ_answer_kz, b.SubFAQ_name_en));
            }
            for (var i = 0; i < buttons.Count; i++)
            {
                twoMenu.Add(new[] { buttons[i] });
            }
            var submenu = new InlineKeyboardMarkup(twoMenu.ToArray());
            await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: search, replyMarkup: submenu);
        }
        static async Task FIO(Message message)
        {
            await Bot.SendTextMessageAsync(message.Chat.Id, (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Нөміріңізді енгізіңіз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Введите свои номер" : "Enter your number");

        }
        static async Task Phone(Message message)
        {
            await Bot.SendTextMessageAsync(message.Chat.Id, (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Сұрағыңызды енгізіңіз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Введите свои вопрос" : "Enter your question");
        }

        static async Task Question(Message message)
        {
            await Bot.SendTextMessageAsync(message.Chat.Id, (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Сізге жақын арада жауап беріледі" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Вам скоро ответят" : "You will be answered soon");
            Record_Question.Record_question(Convert.ToInt32(message.Chat.Id), your_question, fio, phone);
            //await SendInlineKeyboard(message);
        }
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            var message = callbackQueryEventArgs.CallbackQuery.Message;
            if ((callbackQueryEventArgs.CallbackQuery.Data == "KZ") || (callbackQueryEventArgs.CallbackQuery.Data == "RU") || (callbackQueryEventArgs.CallbackQuery.Data == "EN"))
            {
                if (langchat.ContainsKey(Convert.ToInt32(message.Chat.Id)))
                {
                    langchat[Convert.ToInt32(message.Chat.Id)] = callbackQueryEventArgs.CallbackQuery.Data;
                }
                else
                {
                    langchat.Add(Convert.ToInt32(message.Chat.Id), callbackQueryEventArgs.CallbackQuery.Data);
                }
            }
            int number;
            bool success = Int32.TryParse(callbackQueryEventArgs.CallbackQuery.Data, out number);
            string callback_answer = Convert.ToString(callbackQueryEventArgs.CallbackQuery.Data);
            if ((callbackQueryEventArgs.CallbackQuery.Data == "KZ") || (callbackQueryEventArgs.CallbackQuery.Data == "RU") || (callbackQueryEventArgs.CallbackQuery.Data == "EN"))
            {
                string str = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "category_name_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "category_name_ru" : "category_name_en");
                General_language = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "faq_name_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "faq_name_ru" : "faq_name_en");
                answer_language = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "faq_answer_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "faq_answer_ru" : "faq_answer_en");
                my_question = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "Өзімнің сұрағымды қою" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "Задать свой вопрос" : "Ask your question");
                search = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "Сұрақ Іздеу" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "Поиск вопроса" : "Search question");
                submenu_name = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "Submenu_name_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "Submenu_name_ru" : "Submenu_name_en");
                submenu_answer_lang = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "Submenu_answer_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "Submenu_answer_ru" : "Submenu_answer_en");
                subfaq = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "SubFAQ_name_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "SubFAQ_name_ru" : "SubFAQ_name_en");
                subfaq_answer = (callbackQueryEventArgs.CallbackQuery.Data == "KZ") ? "SubFAQ_answer_kz" : ((callbackQueryEventArgs.CallbackQuery.Data == "RU") ? "SubFAQ_answer_ru" : "SubFAQ_answer_en");
                var list_category = Category.GetCategory(str);
                var twoMenu = new List<InlineKeyboardButton[]>();
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                foreach (var b in list_category)
                {
                    buttons.Add(InlineKeyboardButton.WithCallbackData(b.category_name, (b.Id).ToString()));
                }
                for (var i = 0; i < buttons.Count; i++)
                {
                    twoMenu.Add(new[] { buttons[i] });
                }
                var menu = new InlineKeyboardMarkup(twoMenu.ToArray());
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Санатты таңдаңыз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Выберите категорию" : "Choose category", replyMarkup: menu);

                //var list_submenu = Submenu.GetSubmenu(submenu_name);
                //var twoMenu1 = new List<KeyboardButton[]>();
                //var twoMenu2 = new List<KeyboardButton>();
                //foreach (var b in list_submenu)
                //{
                //    twoMenu2.Add(new KeyboardButton(b.Submenu_name));
                //}
                //for (var i = 0; i < twoMenu2.Count; i++)
                //{
                //    twoMenu1.Add(new[] { twoMenu2[i] });
                //}
                //var submenu = new ReplyKeyboardMarkup(twoMenu1.ToArray());
                //await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: "c", replyMarkup: submenu);
            }
            else if ((message.Text == "Выберите категорию")||(message.Text == "Choose category")|| (message.Text == "Санатты таңдаңыз"))
            {
                int id = Convert.ToInt32(callbackQueryEventArgs.CallbackQuery.Data);
                var list_faq = FAQ.GetFAQ(General_language, id);
                var twoMenu = new List<InlineKeyboardButton[]>();
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                foreach (var b in list_faq)
                {
                    buttons.Add(InlineKeyboardButton.WithCallbackData(b.faq_name, b.faq_name_en));
                }
                //buttons.Add(InlineKeyboardButton.WithCallbackData(my_question, "my_question"));
                //buttons.Add(InlineKeyboardButton.WithCallbackData(search, "search"));
                for (var i = 0; i < buttons.Count; i++)
                {
                    twoMenu.Add(new[] { buttons[i] });
                }
                var submenu = new InlineKeyboardMarkup(twoMenu.ToArray());
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Ішкі санатты таңдаңыз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Выберите подкатегорию" : "Select a subcategory", replyMarkup: submenu);

            }
            else if ((message.Text == "Выберите подкатегорию") || (message.Text == "Select a subcategory") || (message.Text == "Ішкі санатты таңдаңыз"))
            {
                string datestr = (callbackQueryEventArgs.CallbackQuery.Data).ToString();
                int id = FAQ.GetFaqId(datestr);
                var list_faq = SubFAQ.GetFAQ(subfaq, id);
                var twoMenu = new List<InlineKeyboardButton[]>();
                List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
                foreach (var b in list_faq)
                {
                    buttons.Add(InlineKeyboardButton.WithCallbackData(b.SubFAQ_name, b.SubFAQ_name_en));
                }
                buttons.Add(InlineKeyboardButton.WithCallbackData(my_question, "my_question"));
                buttons.Add(InlineKeyboardButton.WithCallbackData(search, "search"));
                for (var i = 0; i < buttons.Count; i++)
                {
                    twoMenu.Add(new[] { buttons[i] });
                }
                var submenu = new InlineKeyboardMarkup(twoMenu.ToArray());
                
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Сұрақты таңдаңыз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Выберите вопрос" : "Choose the question", replyMarkup: submenu);
            }
            else if (callbackQueryEventArgs.CallbackQuery.Data == "my_question")
            {
                step = 1;
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Толық аты-жөніңізді енгізіңіз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Введите ФИО" : "Enter your full namе");
            }
            else if (callbackQueryEventArgs.CallbackQuery.Data == "search")
            {
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: (langchat[Convert.ToInt32(message.Chat.Id)] == "KZ") ? "Сұрақ енгізіңіз" : (langchat[Convert.ToInt32(message.Chat.Id)] == "RU") ? "Введите вопрос" : "Enter a question");
                search_bool = true;
            }
            else if (callbackQueryEventArgs.CallbackQuery.Data == "About")
            {
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: @"Бот поможет Вам в обучении английскому языку.");
            }
            else if (SubFAQ.GetAnswer(subfaq_answer, callback_answer) != null)
            {
                string answer = SubFAQ.GetAnswer(subfaq_answer, callback_answer);
                await Bot.SendTextMessageAsync(chatId: message.Chat.Id, text: answer);
            }

        }

    }
}