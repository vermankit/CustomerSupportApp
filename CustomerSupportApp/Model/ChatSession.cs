namespace CustomerSupportApp.Model
{
    public class ChatSession
    {
        public Guid SessionId { get; set; }
        public string AgentConnectionId { get; set; }
        public string AgentName { get; set; }
        private List<UserSession> UserSessions { set; get; }
    }
}
