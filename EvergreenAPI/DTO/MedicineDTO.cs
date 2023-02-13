using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class MedicineDTO
    {
        public int MedicineId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Uses { get; set; }
        [Required]
        public int MedicineCategoryId { get; set; }
        [Required]
        public int ImageId { get; set; }
    }
}
