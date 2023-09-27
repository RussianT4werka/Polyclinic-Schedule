using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorView.Data
{
    public class SessionManager
    {
        private readonly ProtectedSessionStorage storage;
        static Dictionary<string, int> userSesssion = new();

        public SessionManager(ProtectedSessionStorage storage)
        {
            this.storage = storage;
        }

        public async Task<string> GenerateSessionAsync(int idUser)
        {
            string session = Guid.NewGuid().ToString();
            userSesssion.Add(session, idUser);
            await storage.SetAsync("session", session);
            return session;
        }

        public int? GetUserId(string session)
        {
            if (!userSesssion.ContainsKey(session))
                return null;
            return userSesssion[session];
        }
    }
}
