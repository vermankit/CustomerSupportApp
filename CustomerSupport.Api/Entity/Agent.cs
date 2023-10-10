using CustomerSupport.Api.Common;

namespace CustomerSupport.Api.Entity
{
    public class Agent
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsPresent { get; set; }
        public string ConnectionId { get; set; }
        public Seniority Seniority { get; set; }
        public string Team { get; set; }
        public string Shift { get; set; }
        public int Capacity { get; set; }
        public bool IsOnline => !string.IsNullOrWhiteSpace(ConnectionId);
        public int LiveSession { get; set; }
        public bool IsAvailable => LiveSession <= 10;

    }
}
