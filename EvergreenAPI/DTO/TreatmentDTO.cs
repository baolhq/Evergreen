using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class TreatmentDTO
    {
        public int TreatmentId { get; set; }
        
        public string TreatmentName { get; set; }
        
        public string Method { get; set; }
        
        public int ThumbnailId { get; set; }
    }
}
