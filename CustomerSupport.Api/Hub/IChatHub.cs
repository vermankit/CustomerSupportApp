using CustomerSupport.Api.Model;

namespace CustomerSupport.Api.Hub
{
    public interface IChatHub
    {
        Task AssignAgent(AgentDto agentDto);
        Task ReceiveMessage(string message);
        Task ReceiveMessageWithUserId(Guid userId,string message);

    }
}
