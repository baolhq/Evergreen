using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class DiseaseDTO
    {
        public int DiseaseId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Identification { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Affect { get; set; }
        [Required]
        public int DiseaseCategoryId { get; set; }
        [Required]
        public int ThumbnailId { get; set; }
    }
}
