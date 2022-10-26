using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class PlantCategoryRepository : IPlantCategoryRepository
    {
        private readonly AppDbContext _context;

        public PlantCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreatePlantCategory(PlantCategory plantCategory)
        {
            _context.Add(plantCategory);
            return Save();
        }

        public bool DeletePlantCategory(PlantCategory plantCategory)
        {
            _context.Remove(plantCategory);
            return Save();
        }

        public bool PlantCategoryExist(int id)
        {
            return _context.PlantCategories.Any(f => f.PlantCategoryId == id);
        }

        public PlantCategory GetPlantCategory(int id)
        {
            return _context.PlantCategories.Where(s => s.PlantCategoryId == id).FirstOrDefault();
        }

        public ICollection<PlantCategory> GetPlantCategories()
        {
            return _context.PlantCategories.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePlantCategory(PlantCategory plantCategory)
        {
            _context.Update(plantCategory);
            return Save();
        }
    }
}
