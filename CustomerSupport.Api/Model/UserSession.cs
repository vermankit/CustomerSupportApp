using CustomerSupport.Api.Common;

namespace CustomerSupport.Api.Model
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public string ClientConnectionId { get; set; }
        public string CustomerName { get; set; }
        public Status Status { get; set; }
    }
}
