namespace CustomerSupportApp.Model
{
    public class UserSession
    {
        public Guid SessionId { get; set; }
        public string ClientConnectionId { get; set; }
        public string CustomerName { get; set; }
        public Status Status { get; set; }
    }
}
