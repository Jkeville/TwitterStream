using Blazor.UI.Server.Hubs;
using Blazor.UI.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TweetStream.Models.Models;
using TwitterStream.Contracts;

namespace Blazor.UI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IHubContext<TwitterHub> _hub;
        private readonly TimerManager _timer;
        private readonly IDataManager _dataManager;

        public ChartController(IHubContext<TwitterHub> hub, TimerManager timer,IDataManager dataManager)
        {
            _hub = hub;
            _timer = timer;
            _dataManager= dataManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            

            if (!_timer.IsTimerStarted)
                _timer.PrepareTimer(async ()  => await _hub.Clients.All.SendAsync("ReceiveMessage", await _dataManager.GetData()));

            return Ok(new { Message = "Request Completed" });
        }
    }
}
