using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public void DeleteMessage(Message m)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var m1 = context.Messages.SingleOrDefault(
                        c => c.MessageId == m.MessageId);
                    context.Messages.Remove(m1);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }





        public Message GetMessageById(int id)
        {
            Message m = new Message();
            try
            {
                using (var context = new AppDbContext())
                {
                    m = context.Messages.SingleOrDefault(x => x.MessageId == id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return m;
        }





        public List<Message> GetMessages()
        {
            var listMessages = new List<Message>();
            try
            {
                using (var context = new AppDbContext())
                {
                    listMessages = context.Messages.ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listMessages;
        }




        public void SaveMessage(Message m)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Messages.Add(m);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(m);
                throw new Exception(e.Message);
            }
        }



        public void UpdateMessage(Message m)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Entry<Message>(m).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
