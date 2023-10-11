namespace CustomerSupport.Api.Model
{
    public class ChatSession
    {
        public Guid SessionId { get; set; }
        public string AgentConnectionId { get; set; }
        public string AgentName { get; set; }
        public Guid AgentId { get; set; }
        public Guid UserId { get; set; }
        public string ClientConnectionId { get; set; }
        public string CustomerName { get; set; }
        public bool IsActive { get; set; }


        //public List<UserSession> UserSessions { set; get; }
    }
}
