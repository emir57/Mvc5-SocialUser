using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IChatMessageService
    {
        Task<List<ChatMessage>> GetMessages(string user1, string user2);
        Task AddMessages(ChatMessage message);
        Task DeleteMessage(ChatMessage message);

    }
}
