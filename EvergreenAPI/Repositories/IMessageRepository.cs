using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IMessageRepository
    {
        void SaveMessage(Message m);

        Message GetMessageById(int id);

        void DeleteMessage(Message m);

        void UpdateMessage(Message m);

        List<Message> GetMessages();
    }
}
