using System.Text;
using System.Threading.Tasks;
using CountBot.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using TupidBot.Configuration;
using TupidBot.Controllers;
using TupidBot.Services;

namespace TupidBot;
class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var host = new HostBuilder().ConfigureServices((hostContext, services) => ConfigureServices(services)).UseConsoleLifetime().Build();
        Console.WriteLine("Service is active.. ");
        await host.RunAsync();
        Console.WriteLine("You've been served.. ");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();
        services.AddSingleton(appSettings);
        services.AddSingleton<MessageHandler>();
        services.AddSingleton<IStorage, MemoryStorage>();
        services.AddTransient<DefaultMessageController>();
        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();

        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(appSettings.BotToken));
        services.AddHostedService<Bot>();
    }
    static AppSettings BuildAppSettings() { return new AppSettings() { BotToken = "7613509175:AAEosvBHtWc6d-pVobloqf9QmTCiPQQGDKc" }; }
}