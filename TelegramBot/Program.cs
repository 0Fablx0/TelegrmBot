using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using TelegramBot;

namespace TelegramBotExperiments
{

    class Program
    {
        

        static ITelegramBotClient bot = new TelegramBotClient("6022966844:AAFctYwnOdvH1saR-9BBFe67PuZo0SznoQY");
        static MathGame _game = null;

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                var text = message.Text?.ToLower();

                if (text == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "/game - Math game");
                    return;
                }
                if (text == "/game")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Lets start.\n To finish - /endgame");
                    _game = new MathGame();
                    await botClient.SendTextMessageAsync(message.Chat, _game.getQuestion());
                    return;
                }
                if (_game != null)
                {
                    if (text == "/endgame")
                    {
                        await botClient.SendTextMessageAsync(message.Chat, $"Game over\nRight : {_game.right} Wrong : {_game.wrong}\nScore : {_game.right - _game.wrong}");
                        _game = null;
                        return;
                    }
                    if (text == _game.answer.ToString())
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "True");
                        _game.right++;
                        await botClient.SendTextMessageAsync(message.Chat, _game.getQuestion());
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat, "False");
                        _game.wrong++;
                        await botClient.SendTextMessageAsync(message.Chat, _game.getQuestion());
                        return;
                    }
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