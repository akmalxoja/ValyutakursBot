using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using System.Threading;
using File = System.IO.File;
using ValyutakursBot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Requests;

namespace ValyutakursBot
{
    internal class TelegramBotHandler
    {
        public long userid;
        public string Token { get; set; }
        public object currenttime = DateTime.Now.ToString("HH:mm");

        public TelegramBotHandler(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient($"{this.Token}");

            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
                );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();



            cts.Cancel();

        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            var Handlar = update.Type switch
            {
                UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
                UpdateType.EditedMessage => HandleEditMessageAsync(botClient, update, cancellationToken),
                UpdateType.CallbackQuery => HandleCallbackQueryAsync(botClient, update, cancellationToken),
                _ => HandleMessageAsync(botClient, update, cancellationToken),

            };


            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;



            var chatId = message.Chat.Id;

            string filepath = @"C:\Users\VICTUS\Desktop\.Net\RamozonTaqvimBot\users.txt";
            var user_message = $"Received a '{messageText}' message in chat {chatId}. UserName =>  {message.Chat.Username} at {currenttime}\n";
            System.IO.File.AppendAllText(filepath, user_message);
            await Console.Out.WriteLineAsync(user_message);

            await Console.Out.WriteLineAsync(message.Chat.Username);
        }


        private async Task HandleCallbackQueryAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {


            if (update.CallbackQuery.Data == "dollor")
            {
                
                var money = Convertion.ConvertionMoney(Convertion.ConnectWithJson(), "USD");
                Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: userid,
                text: $"1  Dollor  {money}  so'm",
                cancellationToken: cancellationToken) ;

            }


            if (update.CallbackQuery.Data == "evro")
            {
                var money = Convertion.ConvertionMoney(Convertion.ConnectWithJson(), "EUR");
                Message sentMessage2 = await botClient.SendTextMessageAsync(
                chatId: userid,
                text: $"1  Evro  {money}  so'm",
                cancellationToken: cancellationToken);
            }

        }



        private object HandleEditMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            userid = update.Message.Chat.Id;

            if (update.Message.Text == "/start")
            {

                InlineKeyboardMarkup inlineKeyboard = new(new[]
{
                new []
                {
                    InlineKeyboardButton.WithCallbackData(text: "Dollor", callbackData: "dollor"),
                    InlineKeyboardButton.WithCallbackData(text: "Evro", callbackData: "evro"),

                },


            });



                Message sentMessage = await botClient.SendTextMessageAsync(
                   chatId: update.Message.Chat.Id,
                   text: "Valyutalardan birini tanlang 👇",
                   replyMarkup: inlineKeyboard,
                   cancellationToken: cancellationToken);

            }
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()

            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
