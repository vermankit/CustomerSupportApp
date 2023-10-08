using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api.Services.HostedService
{
    public class ShiftManagerService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public ShiftManagerService(IServiceScopeFactory serviceScopeFactory)
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
