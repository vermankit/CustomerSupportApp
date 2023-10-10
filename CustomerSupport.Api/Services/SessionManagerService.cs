using CustomerSupport.Api.Common;
using CustomerSupport.Api.Model;
using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api.Services
{
    public class SessionManagerService : ISessionManagerService
    {
        private static readonly Queue<UserSession> UserSessions = new();
        private static readonly List<ChatSession> ChatSessions = new();
        private readonly IAgentService _agentService;
        public SessionManagerService(IAgentService agentService)
        {
            _agentService = agentService;
        }

        public Guid AddUserSession(string user)
        {
            var capacity = Convert.ToInt32(_agentService.GetConcurrentChatCapacity() * 1.5);
            if (UserSessions.Count >= capacity)
            {
                return Guid.Empty;
            }

            var session = new UserSession
            {
                CustomerName = user,
                Id = Guid.NewGuid(),
                Status = Status.Queued
            };

            UserSessions.Enqueue(session);
            return session.Id;
        }



        public void CreateChatRoom()
        {
            var concurrentChat = _agentService.GetConcurrentChatCapacity();
            var liveChatSession = ChatSessions.Count();

            if (UserSessions.Any() && liveChatSession < concurrentChat)
            {
                var lastUserSession = UserSessions.Dequeue();
                var agent = _agentService.GetAvailableAgent();
                if (agent != null)
                {
                    var chatSession = new ChatSession
                    {
                        AgentConnectionId = agent.ConnectionId,
                        AgentName = agent.Name,
                        ClientConnectionId = lastUserSession.ClientConnectionId,
                        CustomerName = lastUserSession.CustomerName,
                        AgentId = agent.Id,
                        UserId = lastUserSession.Id,
                        SessionId = Guid.NewGuid(),
                    };
                    ChatSessions.Add(chatSession);
                }
            }
        }

        public AgentDto ConnectToAgent(string contextConnectionId,Guid userId)
        {
            var chatSession = ChatSessions.FirstOrDefault(session => session.UserId == userId);
            if (chatSession == null) return null;
            chatSession.ClientConnectionId = contextConnectionId;
            return new AgentDto
            {
                Id = chatSession.AgentId,
                Name = chatSession.AgentName,
                ConnectionId = chatSession.AgentConnectionId
            };

        }

        public string GetUserConnectionId(Guid userId)
        {
            var chatSession = ChatSessions.FirstOrDefault(session => session.UserId == userId);
            if (chatSession == null)
            {
                throw new InvalidOperationException("InActiveUser");
            }
            return chatSession.ClientConnectionId;
        }

        public string GetAgentConnectionId(Guid agentId)
        {
            var chatSession = ChatSessions.FirstOrDefault(session => session.AgentId == agentId);
            if (chatSession == null)
            {
                throw new InvalidOperationException("InActiveAgent");
            }
            return chatSession.AgentConnectionId;
        }

        public void UpdateAgentConnectionId(string contextConnectionId, Guid agentId)
        {
            var activeSession = ChatSessions.Where(session => session.AgentId == agentId);
            foreach (var session in activeSession)
            {
                session.AgentConnectionId = contextConnectionId;
            }
        }

        public List<UserDto> GetActiveUser(Guid agentId)
        {
            return ChatSessions.Where(session => session.AgentId == agentId).Select((c) => new UserDto()
            {
                Id = c.UserId,
                Name = c.CustomerName
            }).ToList();
        }

        public void RemoveActiveSession(string contextConnectionId)
        {
            ChatSessions.RemoveAll(session => session.ClientConnectionId == contextConnectionId);
        }
    }
}
