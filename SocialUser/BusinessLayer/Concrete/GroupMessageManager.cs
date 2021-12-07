using BusinessLayer.Abstract;
using BusinessLayer.Utilities.ValidationTool;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class GroupMessageManager : IGroupMessageService
    {

        private IGroupMessageDal _message;
        private IValidator<GroupMessage> _groupMessageValidator;
        public GroupMessageManager(IGroupMessageDal message, IValidator<GroupMessage> groupMessageValidator)
        {
            _message = message;
            _groupMessageValidator = groupMessageValidator;
        }

        public async Task Add(GroupMessage groupMessage)
        {
            if (ValidationTool.Validate(_groupMessageValidator,groupMessage))
            {
                await _message.Insert(groupMessage);
            }

        }

        public async Task Delete(GroupMessage groupMessage)
        {
            await _message.Delete(groupMessage);
        }

        public async Task<List<GroupMessage>> GetMessages(Expression<Func<GroupMessage, bool>> filter = null)
        {
            return filter == null ?
                await _message.List() :
                await _message.List(filter);
        }
    }
}
