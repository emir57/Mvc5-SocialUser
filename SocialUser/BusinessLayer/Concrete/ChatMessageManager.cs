using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ChatMessageManager : IChatMessageService
    {

        IChatMessageDal _messages;
        public ChatMessageManager(IChatMessageDal message)
        {
            _messages = message;
        }

        public async Task AddMessages(ChatMessage message)
        {
            await _messages.Insert(message);
        }

        public async Task DeleteMessage(ChatMessage message)
        {
            await _messages.Delete(message);
        }

        public async Task<List<ChatMessage>> GetMessages(string user1, string user2)
        {
            return await _messages.OrderedDateTimeAsc(a => a.MessageDateTime, a=>(a.SenderMessageUser==user1 && a.RecipientMessageUser==user2)||(a.SenderMessageUser==user2&&a.RecipientMessageUser==user1));
        }
    }
}
