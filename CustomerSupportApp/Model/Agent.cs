namespace CustomerSupportApp.Model
{
    public class Agent
    {
        public string Id { get; set; }
        public bool IsAvailable { get; set; }
        public string ConnectionId { get; set; }
        public Seniority Seniority { get; set; }
        public string Team { get; set; }
        public string Shift { get; set; }
        public bool IsOnline => !string.IsNullOrWhiteSpace(ConnectionId);
    }
}
