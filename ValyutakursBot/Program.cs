using Telegram.Bot;
using ValyutakursBot;

namespace  ValyutakursBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            const string token = "7033156784:AAH1DvKtUeau6V8rk2VSOZHn4OcRw78MXGs";
            TelegramBotHandler telegramBotHandler = new TelegramBotHandler(token);

            try
            {
                await telegramBotHandler.BotHandle();
            }
            catch (Exception ex)
            {
                throw new Exception("No Error");
            }
        }
    }
}