using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class DiseaseCategoryDTO
    {
        public int DiseaseCategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
