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
                agent.Capacity = Convert.ToInt32(_seniorityCapacityMapping[agent.Seniority] * 10);
                agent.IsPresent = true;
            }

            AssignCapacity();

        }

        private void AssignCapacity()
        {

            var lead = _agents.Count(agent => agent.Seniority == Seniority.Lead && agent.IsPresent);
            var senior = _agents.Count(agent => agent.Seniority == Seniority.Senior && agent.IsPresent);
            var mid = _agents.Count(agent => agent.Seniority == Seniority.Mid && agent.IsPresent);
            var junior = _agents.Count(agent => agent.Seniority == Seniority.Junior && agent.IsPresent);

            _teamCapacity = (lead * _seniorityCapacityMapping[Seniority.Lead])
                            + (senior * _seniorityCapacityMapping[Seniority.Senior])
                            + (mid * _seniorityCapacityMapping[Seniority.Mid])
                            + (junior * _seniorityCapacityMapping[Seniority.Junior]);

        }

        public int GetConcurrentChatCapacity()
        {
            return Convert.ToInt32(_teamCapacity * 10);
        }

        public Guid UpdateAgentStatus(Guid agentId)
        {
            var agent = _agents.FirstOrDefault(agent => agent.Id.Equals(agentId));
            return agent.Id;
        }

        public Guid GetAgentId(string name)
        {
            var agent = _agents.FirstOrDefault(agent => agent.Name.Equals(name) && agent.IsPresent);
            if (agent == null)
            {
                return Guid.Empty;
            }

            return agent.Id;
        }

        public void UpdateConnectionId(string contextConnectionId, Guid agentId)
        {
            var agent = _agents.FirstOrDefault(agent => agent.Id.Equals(agentId));
            agent.ConnectionId = contextConnectionId;
        }

        public Agent GetAvailableAgent()
        {
            var availableAgents = _agents
                .Where(agent => agent.IsPresent && agent.IsOnline && agent.IsAvailable)
                .OrderByDescending(agent => agent.Seniority)
                .ToList();

         
            var juniorAgent = availableAgents.FirstOrDefault(agent => agent.Seniority == Seniority.Junior);
            if (juniorAgent != null)
            {
                ++juniorAgent.LiveSession;
                return juniorAgent;
            }

            var midLevelAgent = availableAgents.FirstOrDefault(agent => agent.Seniority == Seniority.Mid);
            if (midLevelAgent != null)
            {
                ++midLevelAgent.LiveSession; 
                return midLevelAgent;
            }

         
            var seniorAgent = availableAgents.FirstOrDefault(agent => agent.Seniority == Seniority.Senior);
            if (seniorAgent != null)
            {
                ++seniorAgent.LiveSession; 
                return seniorAgent;
            }

            var leadAgent = availableAgents.FirstOrDefault(agent => agent.Seniority == Seniority.Lead);
            if (leadAgent != null)
            {
                ++leadAgent.LiveSession;
                return leadAgent;
            }

            return null;
        }

    }
}
