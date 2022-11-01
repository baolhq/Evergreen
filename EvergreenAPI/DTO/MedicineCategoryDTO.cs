using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class MedicineCategoryDTO
    {
        public int MedicineCategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
