using Microsoft.AspNetCore.SignalR;

namespace MyWebApi.Hubs
{
    public class ChatHub : Hub
    {
        private static List<string> ConnectedUsers = new List<string>();
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public override Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext()?.Request.Query["username"].ToString();
            if (!string.IsNullOrEmpty(username))
            {
                ConnectedUsers.Remove(username);
            }

            Clients.All.SendAsync("UpdateUserList", ConnectedUsers);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User.Identity.Name;
            ConnectedUsers.Remove(username);
            Clients.All.SendAsync("UpdateUserList", ConnectedUsers);
            return base.OnDisconnectedAsync(exception);
        }
    }
}