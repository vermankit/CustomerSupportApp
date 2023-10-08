using CustomerSupport.Api.Common;
using CustomerSupport.Api.Entity;
using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api.Services
{
    public class AgentService : IAgentService
    {
        private readonly List<Agent> _agents;
        private double _teamCapacity;
        private readonly Dictionary<Seniority, double> _seniorityCapacityMapping;
        public AgentService(List<Agent> agents)
        {
            _agents = agents;
            _seniorityCapacityMapping = new Dictionary<Seniority, double>()
            {
                {Seniority.Lead,.5},{Seniority.Mid,.6},{Seniority.Senior,.8},{Seniority.Junior,.4},

            };
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

            AssignCapacity();

        }

        private void AssignCapacity()
        {

            var lead = _agents.Count(agent => agent.Seniority == Seniority.Lead && agent.IsAvailable);
            var senior = _agents.Count(agent => agent.Seniority == Seniority.Senior && agent.IsAvailable);
            var mid = _agents.Count(agent => agent.Seniority == Seniority.Mid && agent.IsAvailable);
            var junior = _agents.Count(agent => agent.Seniority == Seniority.Junior && agent.IsAvailable);

            _teamCapacity = (lead * _seniorityCapacityMapping[Seniority.Lead])
                            + (senior * _seniorityCapacityMapping[Seniority.Senior])
                            + (mid * _seniorityCapacityMapping[Seniority.Mid])
                            + (junior * _seniorityCapacityMapping[Seniority.Junior]);

        }

        public int GetConcurrentChatCapacity()
        {
            return Convert.ToInt32(_teamCapacity * 10);
        }
    }
}
