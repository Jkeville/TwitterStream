using Microsoft.AspNetCore.SignalR;
using TweetStream.Models.Models;

namespace Blazor.UI.Server.Hubs
{
    public class TwitterHub : Hub
    {
        public async Task SendMessage(TopTenTotalsModel data)
        {
            await Clients.All.SendAsync("ReceiveMessage", data);
        }
    }
}
