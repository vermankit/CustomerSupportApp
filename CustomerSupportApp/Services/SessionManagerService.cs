using CustomerSupportApp.Model;
using CustomerSupportApp.Services.Interface;

namespace CustomerSupportApp.Services
{
    public class SessionManagerService : ISessionManagerService
    {
        private readonly Queue<UserSession> _userSessionsQueue;
        private readonly List<ChatSession> _chatSessionsList;
        public SessionManagerService(List<ChatSession> chatSessions, Queue<UserSession> userSessionsQueue)
        {
            _chatSessionsList = chatSessions;
            _userSessionsQueue = userSessionsQueue;
        }
    }
}
