using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class PlantCategoryDTO
    {
        public int PlantCategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
