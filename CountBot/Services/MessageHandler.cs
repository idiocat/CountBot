using CountBot.Utilities;
using Telegram.Bot;
using Telegram.Bot.Types;
using TupidBot.Services;

namespace CountBot.Services;

public class MessageHandler
{ 
    private readonly ITelegramBotClient _telegramClient;
    private readonly IStorage _memoryStorage;
    public MessageHandler (ITelegramBotClient telegramClient, IStorage memoryStorage)
    {
        _telegramClient = telegramClient;
        _memoryStorage = memoryStorage;
    }
    public async Task Handle(Message message, CancellationToken cancellationToken)
    {
        string counter;
        try
        {
            switch (_memoryStorage.GetSession(message.Chat.Id).Account)
            {
                case "cs":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Your message has {SymbolCounter.Count(message.Text)} symbols.", cancellationToken: cancellationToken);
                    break;
                case "cn":
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"The sum of your numbers is {NumberCounter.Count(message.Text)}.", cancellationToken: cancellationToken);
                    break;
                default: throw new ArithmeticException();
            }
        }
        catch (Exception ex) { await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"This was unaccounted for!{Environment.NewLine}{ex.Message}", cancellationToken: cancellationToken); }
    }
}
