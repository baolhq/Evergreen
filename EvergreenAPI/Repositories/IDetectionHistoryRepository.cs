using EvergreenAPI.DTO;
using EvergreenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvergreenAPI.Repositories
{
    public interface IDetectionHistoryRepository
    {
        ICollection<DetectionHistory> GetDetectionHistories(int accountId);
        ICollection<DetectionHistory> GetAll();
        DetectionHistory GetDetectionHistory(int id);
        bool Exist(int id);
    }
}
