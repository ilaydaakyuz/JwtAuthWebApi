using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MyWebApi.Hubs
{
    public class ChatHub : Hub
    {
        // Kullanıcı adları ve bağlantı ID'leri arasında ilişkiyi takip etmek için
        private static ConcurrentDictionary<string, List<string>> UserConnections = new ConcurrentDictionary<string, List<string>>();

        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task SendPrivateMessage(string receiver, string message)
        {   
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            
            if (UserConnections.ContainsKey(receiver))
            {
             
                var connections = UserConnections[receiver];
                
                foreach (var connectionId in connections)
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage",username,message);
                }
            }
        }

        public override Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            if (!string.IsNullOrEmpty(username))
            {
                // Eğer kullanıcı listede değilse ekle
                if (!UserConnections.ContainsKey(username))
                {
                    UserConnections[username] = new List<string>();
                }

                // Eğer kullanıcıya ait bu bağlantı zaten eklenmemişse, yeni bağlantıyı ekle
                if (!UserConnections[username].Contains(Context.ConnectionId))
                {
                    UserConnections[username].Add(Context.ConnectionId);
                }
            }

            // Tüm kullanıcılara güncel kullanıcı listesini gönder
            Clients.All.SendAsync("UpdateUserList", UserConnections.Keys.ToList());
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            if (!string.IsNullOrEmpty(username))
            {
                // Kullanıcının bağlantı ID'sini kaldır
                if (UserConnections.ContainsKey(username))
                {
                    UserConnections[username].Remove(Context.ConnectionId);

                    // Eğer bu kullanıcının başka bağlantısı yoksa listeden çıkar
                    if (UserConnections[username].Count == 0)
                    {
                        UserConnections.TryRemove(username, out _);
                    }
                }
            }

            // Tüm kullanıcılara güncel kullanıcı listesini gönder
            Clients.All.SendAsync("UpdateUserList", UserConnections.Keys.ToList());
            return base.OnDisconnectedAsync(exception);
        }
    }
}
