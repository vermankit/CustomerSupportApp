using CustomerSupport.Api.Model;

namespace CustomerSupport.Api.Services.Interface
{
    public interface ISessionManagerService
    {
        Guid AddUserSession(string user);
        void CreateChatRoom();
        AgentDto ConnectToAgent(string userConnectionId, Guid userid);
        string GetUserConnectionId(Guid userId);
        string GetAgentConnectionId(Guid agentId);
    }
}
