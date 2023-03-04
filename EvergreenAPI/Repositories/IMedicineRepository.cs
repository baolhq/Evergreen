using EvergreenAPI.Models;
using System.Collections.Generic;

namespace EvergreenAPI.Repositories
{
    public interface IMedicineRepository
    {
        ICollection<MedicineCategory> GetMedicineCategories();
        ICollection<Image> GetImages();
        ICollection<Disease> GetDiseases();
        ICollection<Medicine> GetMedicines();
        Medicine GetMedicine(int id);
        bool MedicineExist(int id);
        bool CreateMedicine(Medicine medicine);
        bool UpdateMedicine(Medicine medicine);
        bool DeleteMedicine(Medicine medicine);
        bool Save();
    }
}
