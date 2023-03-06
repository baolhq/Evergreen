using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public class DetectionHistoryRepository : IDetectionHistoryRepository
    {
        private readonly AppDbContext _context;

        public DetectionHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public DetectionHistory GetDetectionHistory(int id)
        {
            return _context.DetectionHistories.Where(s => s.DetectionHistoryId == id).FirstOrDefault(); ;
        }

        public ICollection<DetectionHistory> GetDetectionHistories(int accountId)
        {
            return _context.DetectionHistories.Where(d => d.AccountId == accountId).ToList();
        }

        public bool Exist(int id) => _context.DetectionHistories.Any(f => f.DetectionHistoryId == id);
    }
}
