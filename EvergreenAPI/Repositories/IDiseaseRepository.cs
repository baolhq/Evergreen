using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IDiseaseRepository
    {
        ICollection<Disease> GetDiseases();
        ICollection<DiseaseCategory> GetDiseaseCategories();
        ICollection<Image> GetImages();
        Disease GetDisease(int id);
        bool DiseaseExist(int id);
        bool CreateDisease(Disease disease);
        bool UpdateDisease(Disease disease);
        bool DeleteDisease(Disease disease);
        bool Save();
    }
}
