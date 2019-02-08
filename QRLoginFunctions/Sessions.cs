using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace QRLoginFunctions
{
    internal static class Sessions
    {
        private static MemoryCache _sessions = new MemoryCache("QRLoginSessions");

        public static Session GetOrCreateSession(string key)
        {
            var session = new Session();
            var resultSession = (Session) _sessions.AddOrGetExisting(key, session, DateTimeOffset.UtcNow.AddSeconds(30))
                                ?? session;
            return resultSession;
        }
    }
}
