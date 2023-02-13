using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class DiseaseTreatment
    {
        [Key]
        public int DiseaseTreatmentId { get; set; }
        [ForeignKey("Disease")]
        public int DiseaseId { get; set; }
        [ForeignKey("Treatment")]
        public int? TreatmentId { get; set; }
        public virtual Disease Disease { get; set; }
        public virtual Treatment Treatment { get; set; }
        
    }
}
