using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool DeleteMessage(Message b)
        {
            _context.Remove(b);
            return Save();
        }

        public Message GetMessageById(int id)
        {
            return _context.Messages.Where(s => s.MessageId == id).FirstOrDefault();
        }

        public ICollection<Message> GetMessages()
        {
            return _context.Messages.ToList();
        }

        public bool MessageExist(int id)
        {
            return _context.Messages.Any(f => f.MessageId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool SaveMessage(Message b)
        {
            _context.Add(b);
            return Save();
        }

        public bool UpdateMessage(Message b)
        {
            _context.Update(b);
            return Save();
        }
    }
}
