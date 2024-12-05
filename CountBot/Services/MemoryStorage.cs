using System.Collections.Concurrent;
using TupidBot.Models;

namespace TupidBot.Services;

public class MemoryStorage : IStorage
{
    private readonly ConcurrentDictionary<long, Session> _sessions;
    public MemoryStorage() { _sessions = new ConcurrentDictionary<long, Session>(); }
    public Session GetSession(long chadtID)
    {
        if (_sessions.ContainsKey(chadtID)) { return _sessions[chadtID]; }

        var newSession = new Session() { Account = "cs" };
        _sessions.TryAdd(chadtID, newSession);
        return newSession;
    }
}
