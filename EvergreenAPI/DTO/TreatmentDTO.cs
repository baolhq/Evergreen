using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class TreatmentDTO
    {
        public int TreatmentId { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Method { get; set; }
        [Required]
        public int ThumbnailId { get; set; }
    }
}
