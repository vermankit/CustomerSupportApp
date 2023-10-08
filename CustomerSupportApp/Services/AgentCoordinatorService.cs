using CustomerSupportApp.Services.Interface;

namespace CustomerSupportApp.Services
{
    public class AgentCoordinatorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public AgentCoordinatorService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
