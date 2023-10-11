using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api.Services.HostedService
{
    public class SessionMoniterService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public SessionMoniterService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var workScope = _serviceScopeFactory.CreateScope();
            var sessionManager = workScope.ServiceProvider.GetService<ISessionManagerService>();


            var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                sessionManager?.ClearSession();
            }
        }

    }
}
