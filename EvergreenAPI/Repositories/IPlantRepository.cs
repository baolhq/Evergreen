using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IPlantRepository
    {
        ICollection<Plant> GetPlants();
        Plant GetPlant(int id);
        bool PlantExist(int id);
        bool CreatePlant(Plant plant);
        bool UpdatePlant(Plant plant);
        bool DeletePlant(Plant plant);
        bool Save();
    }
}
