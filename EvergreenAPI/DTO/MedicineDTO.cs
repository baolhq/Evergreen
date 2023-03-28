using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class MedicineDTO
    {
        public int MedicineId { get; set; }
        
        public string Name { get; set; }
        
        public string Uses { get; set; }
        
        public int MedicineCategoryId { get; set; }
        
        public int ThumbnailId { get; set; }
    }
}
