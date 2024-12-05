using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TupidBot.Controllers;

namespace TupidBot;

class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private DefaultMessageController _defaultMessageController;
    private TextMessageController _textMessageController;
    private InlineKeyboardController _inlineKeyboardController;
    public Bot(ITelegramBotClient telegramClient,
        DefaultMessageController defaultMessageController,
        TextMessageController textMessageController,
        InlineKeyboardController inlineKeyboardController)
    {
        _telegramClient = telegramClient;
        _defaultMessageController = defaultMessageController;
        _textMessageController = textMessageController;
        _inlineKeyboardController = inlineKeyboardController;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _telegramClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, new ReceiverOptions() { AllowedUpdates = { } }, cancellationToken: cancellationToken);
        Console.WriteLine("Bot is functioning.. ");
    }
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken: cancellationToken);
            return;
        }
        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text: await _textMessageController.Handle(update.Message, cancellationToken: cancellationToken); return;
                default: await _defaultMessageController.Handle(update.Message, cancellationToken: cancellationToken); return;
            }
        }
    }
    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:{Environment.NewLine}" +
            $"[{apiRequestException.ErrorCode}]{Environment.NewLine}" +
            $"{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(errorMessage);
        Console.WriteLine("Waiting 10 seconds.. ");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}
