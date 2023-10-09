using CustomerSupport.Api.Entity;

namespace CustomerSupport.Api.Services.Interface
{
    public interface IAgentService
    {
        void ChangeShift();
        int GetConcurrentChatCapacity();
        Guid UpdateAgentStatus(Guid agentId);
        Guid GetAgentId(string name);
        void UpdateConnectionId(string contextConnectionId, Guid agentId);
        Agent GetAvailableAgent();
    }
}
