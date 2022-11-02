using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class PlantRepository : IPlantRepository
    {
        private readonly AppDbContext _context;

        public PlantRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreatePlant(Plant plant)
        {
            _context.Add(plant);
            return Save();
        }

        public bool DeletePlant(Plant plant)
        {
            _context.Remove(plant);
            return Save();
        }

        public bool PlantExist(int id)
        {
            return _context.Plants.Any(f => f.PlantId == id);
        }

        public Plant GetPlant(int id)
        {
            return _context.Plants.Include(d => d.PlantCategory).Where(s => s.PlantId == id).FirstOrDefault();
        }

        public ICollection<Plant> GetPlants()
        {
            return _context.Plants.Include(d => d.PlantCategory).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePlant(Plant plant)
        {
            _context.Update(plant);
            return Save();
        }
    }
}
