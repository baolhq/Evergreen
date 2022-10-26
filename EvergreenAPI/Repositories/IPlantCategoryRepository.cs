using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IPlantCategoryRepository
    {
        ICollection<PlantCategory> GetPlantCategories();
        PlantCategory GetPlantCategory(int id);
        bool PlantCategoryExist(int id);
        bool CreatePlantCategory(PlantCategory plantCategory);
        bool UpdatePlantCategory(PlantCategory plantCategory);
        bool DeletePlantCategory(PlantCategory plantCategory);
        bool Save();
    }
}
