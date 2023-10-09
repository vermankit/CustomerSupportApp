using CustomerSupport.Api.Model;
using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api.Hub
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub<IChatHub>
    {
        private readonly ISessionManagerService _sessionManagerService;
        private readonly IAgentService  _agentService;
        public ChatHub(IAgentService agentService, ISessionManagerService sessionManagerService)
        {
            _agentService = agentService;
            _sessionManagerService = sessionManagerService;
        }
        public override Task OnConnectedAsync()
        {
           return  Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return Task.CompletedTask;
        }

        
        public void UpdateAgentConnectionId(Guid agentId)
        {
            _agentService.UpdateConnectionId(Context.ConnectionId, agentId);
            _sessionManagerService.UpdateAgentConnectionId(Context.ConnectionId, agentId);
        }

        /// <summary>
        /// This will be called by Customer window 
        /// </summary>
        /// <param name="userid"></param>
        public AgentDto ConnectToAgent(Guid userid)
        {
            var result =  _sessionManagerService.ConnectToAgent(Context.ConnectionId,userid);
            return result;
        }

        public void GetAssignedCustomers(Guid agentId)
        {
           // _sessionManagerService.GetAssignedCustomers(agentId);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }


        public async Task SendMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        /// <summary>
        /// This will be used to send message to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToUser(Guid userId, string message)
        {
            var connectionId = _sessionManagerService.GetUserConnectionId(userId);
            await Clients.Client(connectionId).ReceiveMessage(message);
        }

        /// <summary>
        /// This will be used to send message to agent
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageToAgent(Guid agentId, string message)
        {
            var connectionId = _sessionManagerService.GetAgentConnectionId(agentId);
            await Clients.Client(connectionId).ReceiveMessage(message);
        }
    }
}
