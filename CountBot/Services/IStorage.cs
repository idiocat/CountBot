using TupidBot.Models;

namespace TupidBot.Services;

public interface IStorage
{
    Session GetSession(long chatID);
}
