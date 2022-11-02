using EvergreenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class DiseaseRepository : IDiseaseRepository
    {
        private readonly AppDbContext _context;

        public DiseaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateDisease(Disease disease)
        {
            _context.Add(disease);
            return Save();
        }

        public bool DeleteDisease(Disease disease)
        {
            _context.Remove(disease);
            return Save();
        }

        public bool DiseaseExist(int id)
        {
            return _context.Diseases.Any(f => f.DiseaseId == id);
        }

        public Disease GetDisease(int id)
        {   
            return _context.Diseases.Include(d => d.DiseaseCategory).Include(d => d.Image).Where(s => s.DiseaseId == id).FirstOrDefault(); ;
        }

        public ICollection<Disease> GetDiseases()
        {
            return _context.Diseases.Include(d => d.DiseaseCategory).Include(d => d.Image).ToList();
        }

        public ICollection<Image> GetImages()
        {
            return _context.Images.ToList();
        }

        public ICollection<DiseaseCategory> GetDiseaseCategories()
        {
            return _context.DiseaseCategories.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateDisease(Disease disease)
        {
            _context.Update(disease);
            return Save();
        }
    }
}
