using Telegram.Bot;
using Telegram.Bot.Types;
using TupidBot.Services;

namespace TupidBot.Controllers;

public class InlineKeyboardController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;
    public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _memoryStorage = memoryStorage;
    }
    public async Task Handle(CallbackQuery? callbackQuery, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Controller {GetType().Name} is activated");
        if (callbackQuery?.Data == null) { return; }
        _memoryStorage.GetSession(callbackQuery.From.Id).Account = callbackQuery.Data;
        string currentMode = callbackQuery.Data switch
        {
            "cn" => "numbers. Enter numbers separated by space.",
            "cs" => "symbols. Enter your message.",
            _ => "nothing. Choose what should be counted."
        };
        await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"Current counting: {currentMode}", cancellationToken: cancellationToken);
    }
}
