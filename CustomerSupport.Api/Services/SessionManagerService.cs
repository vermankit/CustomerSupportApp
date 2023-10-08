﻿using CustomerSupport.Api.Model;
using CustomerSupport.Api.Services.Interface;
using System.Xml.Linq;
using CustomerSupport.Api.Common;

namespace CustomerSupport.Api.Services
{
    public class SessionManagerService : ISessionManagerService
    {
        private static readonly Queue<UserSession> UserSessions = new();
        private static readonly List<ChatSession> ChatSessionsList = new();
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
            var liveChatSession = ChatSessionsList.Count();

            if (liveChatSession < concurrentChat)
            {
                //Assign agent here
            }
        }

        public AgentDto ConnectToAgent(string contextConnectionId,Guid userId)
        {
            var chatSession = ChatSessionsList.FirstOrDefault(session => session.UserId == userId);
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
            var chatSession = ChatSessionsList.FirstOrDefault(session => session.UserId == userId);
            if (chatSession == null)
            {
                throw new InvalidOperationException("InActiveUser");
            }
            return chatSession.ClientConnectionId;
        }

        public string GetAgentConnectionId(Guid agentId)
        {
            var chatSession = ChatSessionsList.FirstOrDefault(session => session.AgentId == agentId);
            if (chatSession == null)
            {
                throw new InvalidOperationException("InActiveAgent");
            }
            return chatSession.AgentConnectionId;
        }
    }
}