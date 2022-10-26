using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IDiseaseRepository
    {
        ICollection<Disease> GetDiseases();
        Disease GetDisease(int id);
        bool DiseaseExist(int id);
        bool CreateDisease(Disease disease);
        bool UpdateDisease(Disease disease);
        bool DeleteDisease(Disease disease);
        bool Save();
    }
}
