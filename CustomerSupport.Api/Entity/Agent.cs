using CustomerSupport.Api.Common;

namespace CustomerSupport.Api.Entity
{
    public class Agent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public string ConnectionId { get; set; }
        public Seniority Seniority { get; set; }
        public string Team { get; set; }
        public string Shift { get; set; }
        public bool IsOnline => !string.IsNullOrWhiteSpace(ConnectionId);
    }
}
