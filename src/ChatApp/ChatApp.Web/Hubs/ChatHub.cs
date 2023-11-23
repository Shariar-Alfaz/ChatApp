using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Web.Hubs
{
    public class ChatHub:Hub
    {
        public static int Count { get; set; }

        public override async Task OnConnectedAsync()
        {
            Count++;
            await Clients.All.SendAsync("userCount", Count);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Count--;
            await Clients.All.SendAsync("userCount", Count);
            await base.OnDisconnectedAsync(exception);
        }

        //public async Task UserCount()
        //{
        //    Count++;
        //    await Clients.All.SendAsync("userCount", Count);
        //}
    }
}
