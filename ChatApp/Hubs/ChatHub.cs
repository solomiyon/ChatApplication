using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public ChatHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            var tmp = Context.User;
            var tlp = Clients.All;
            await base.OnConnectedAsync();
        }
    }
}
