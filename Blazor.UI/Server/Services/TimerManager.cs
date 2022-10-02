namespace Blazor.UI.Server.Services
{
    public class TimerManager
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        private IConfiguration _config;
        private int timerSendRateMS;
        private int timerRunMins;
        public DateTime TimerStarted { get; set; }

        public bool IsTimerStarted { get; set; }

        public void PrepareTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, timerSendRateMS, timerSendRateMS);
            TimerStarted = DateTime.Now;
            IsTimerStarted = true;
        }

        public void Execute(object stateInfo)
        {
            _action();

            if ((DateTime.Now - TimerStarted).Minutes > timerRunMins)
            {
                IsTimerStarted = false;
                _timer.Dispose();
            }
        }

        public TimerManager(IConfiguration config)
        {
            _config=config;
            timerSendRateMS =
                config.GetSection("ClientRefresh").GetValue<int>("RefreshMS");
            timerRunMins= config.GetSection("ClientRefresh").GetValue<int>("MaxRefreshRunMinutes");

        }
    }
}
