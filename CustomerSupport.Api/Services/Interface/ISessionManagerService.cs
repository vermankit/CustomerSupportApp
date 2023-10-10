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
        void UpdateAgentConnectionId(string contextConnectionId, Guid agentId);
        List<UserDto> GetActiveUser(Guid agentId);
        void RemoveActiveSession(string contextConnectionId);
    }
}
