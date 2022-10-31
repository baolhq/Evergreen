using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IMessageRepository
    {
        bool SaveMessage(Message b);

        Message GetMessageById(int id);

        bool DeleteMessage(Message b);

        bool UpdateMessage(Message b);

        ICollection<Message> GetMessages();
        bool MessageExist(int id);
        bool Save();
    }
}
