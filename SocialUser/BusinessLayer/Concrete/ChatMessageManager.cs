using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class ChatMessageManager : IChatMessageService
    {

        private IChatMessageDal _messages;
        private IValidator<ChatMessage> _chatMessageValidator;
        public ChatMessageManager(IChatMessageDal message, IValidator<ChatMessage> chatMessageValidator)
        {
            _messages = message;
            _chatMessageValidator = chatMessageValidator;
        }

        public async Task AddMessages(ChatMessage message)
        {
            if (!(_chatMessageValidator.Validate(message).Errors.Count > 0))
            {
                await _messages.Insert(message);
            }
        }

        public async Task DeleteMessage(ChatMessage message)
        {
            await _messages.Delete(message);
        }

        public async Task<List<ChatMessage>> GetMessages(string user1, string user2)
        {
            return await _messages.OrderedDateTimeAsc(a => a.MessageDateTime, a => (a.SenderMessageUser == user1 && a.RecipientMessageUser == user2) || (a.SenderMessageUser == user2 && a.RecipientMessageUser == user1));
        }
    }
}
