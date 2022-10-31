﻿using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly AppDbContext _context;

        public TreatmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateTreatment(Treatment treatment)
        {
            _context.Add(treatment);
            return Save();
        }

        public bool DeleteTreatment(Treatment treatment)
        {
            _context.Remove(treatment);
            return Save();
        }

        public bool TreatmentExist(int id)
        {
            return _context.Treatments.Any(f => f.TreatmentId == id);
        }

        public Treatment GetTreatment(int id)
        {
            return _context.Treatments.Where(s => s.TreatmentId == id).FirstOrDefault();
        }

        public ICollection<Treatment> GetTreatments()
        {
            return _context.Treatments.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateTreatment(Treatment treatment)
        {
            _context.Update(treatment);
            return Save();
        }
    }
}