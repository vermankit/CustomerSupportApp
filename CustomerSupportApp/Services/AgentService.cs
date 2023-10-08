using CustomerSupportApp.Model;
using CustomerSupportApp.Services.Interface;

namespace CustomerSupportApp.Services
{
    public class AgentService : IAgentService
    {
        private readonly List<Agent> _agents;

        public AgentService(List<Agent> agents)
        {
            _agents = agents;
        }

        private static string GetCurrentShift()
        {

            var earlyDayShiftStartTime = TimeSpan.FromHours(6);
            var midDayShiftStartTime = TimeSpan.FromHours(14);
            var nightShiftStartTime = TimeSpan.FromHours(22);

            var currentTime = DateTime.Now.TimeOfDay;

            if (currentTime >= earlyDayShiftStartTime && currentTime < (earlyDayShiftStartTime + TimeSpan.FromHours(8)))
            {
                return "Morning";
            }

            if (currentTime >= midDayShiftStartTime && currentTime < (midDayShiftStartTime + TimeSpan.FromHours(8)))
            {
                return "Afternoon";
            }

            if (currentTime >= nightShiftStartTime || currentTime < earlyDayShiftStartTime)
            {
                return "Night";
            }

            return "UnknownShift"; 
        }

        public void ChangeShift()
        {
            string currentShift = GetCurrentShift();

            var availableAgents = _agents
                .Where(agent => agent.Shift.Contains(currentShift))
                .ToList();

            foreach (var agent in availableAgents)
            {
                agent.IsAvailable = true; 
            }
        }
    }
}
