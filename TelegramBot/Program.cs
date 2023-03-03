using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBot;
using System.Collections.Generic;

namespace TelegramBotExperiments
{

    class Program
    {
        

        static ITelegramBotClient bot = new TelegramBotClient("6022966844:AAFctYwnOdvH1saR-9BBFe67PuZo0SznoQY");
        static ApiController _api = new ApiController();

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                var text = message.Text?.ToLower();

                if (text == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Write city name");
                    return;
                }
                else if (text !=null)
                {
                    List<string> places = await _api.getInterestingPlacesAsync(text);
                    foreach(var x in places)
                    {
                        await botClient.SendTextMessageAsync(message.Chat, x);
                    }
                    return;
                }
               
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }, 
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }
    }
}