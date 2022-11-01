using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class PlantDTO
    {
        public int PlantId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Description { get; set; }
        [Required]
        public int PlantCategoryId { get; set; }
    }
}
