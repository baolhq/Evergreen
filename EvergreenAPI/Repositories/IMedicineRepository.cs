using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IMedicineRepository
    {
        ICollection<Medicine> GetMedicines();
        Medicine GetMedicine(int id);
        bool MedicineExist(int id);
        bool CreateMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool DeleteMedicine(Medicine medicine);
        bool Save();
    }
}
