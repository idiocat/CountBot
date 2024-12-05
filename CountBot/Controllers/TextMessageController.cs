using CountBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TupidBot.Services;

namespace TupidBot.Controllers;

public class TextMessageController
{
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;
    private readonly MessageHandler _messageHandler;
    public TextMessageController(ITelegramBotClient telegramClient, IStorage memoryStorage, MessageHandler messageHandler)
    {
        _telegramClient = telegramClient;
        _memoryStorage = memoryStorage;
        _messageHandler = messageHandler;
    }
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Controller {GetType().Name} is activated");
        switch (message.Text)
        {
            case "/start":
                List<InlineKeyboardButton[]> buttons = [];
                buttons.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($"Count symbols", $"cs"),
                    InlineKeyboardButton.WithCallbackData($"Count numbers", $"cn")
                });
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>This bot counts.</b>{Environment.NewLine}" +
                    $"You can count on this bot.{Environment.NewLine}" +
                    $"This bot was given a title for counting.{Environment.NewLine}" +
                    $"Call it <i>Count</i> Bot.{Environment.NewLine}",
                    cancellationToken: cancellationToken, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                break;
            default:
                await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Counting is on the counter.", cancellationToken: cancellationToken);
                _messageHandler.Handle(message, cancellationToken);
                break;
        }
    }
}
