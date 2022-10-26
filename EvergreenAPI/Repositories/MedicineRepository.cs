using EvergreenAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace EvergreenAPI.Repositories
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly AppDbContext _context;

        public MedicineRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateMedicine(Medicine medicine)
        {
            _context.Add(medicine);
            return Save();
        }

        public bool DeleteMedicine(Medicine medicine)
        {
            _context.Remove(medicine);
            return Save();
        }

        public bool MedicineExist(int id)
        {
            return _context.Medicines.Any(f => f.MedicineId == id);
        }

        public Medicine GetMedicine(int id)
        {
            return _context.Medicines.Where(s => s.MedicineId == id).FirstOrDefault();
        }

        public ICollection<Medicine> GetMedicines()
        {
            return _context.Medicines.ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            _context.Update(medicine);
            return Save();
        }
    }
}
