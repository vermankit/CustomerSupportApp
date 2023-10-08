using CustomerSupportApp.Model;
using CustomerSupportApp.Services.Interface;

namespace CustomerSupportApp.Services
{
    public class ShiftManagerService : BackgroundService
    {
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ShiftManagerService( IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var workScope = _serviceScopeFactory.CreateScope();
            var agentService = workScope.ServiceProvider.GetService<IAgentService>();
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                agentService?.ChangeShift();
            }
        }

    }
}
