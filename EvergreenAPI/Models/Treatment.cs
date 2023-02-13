using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Treatment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TreatmentId { get; set; }
        [Required]
        public string Method { get; set; }
        
        [ForeignKey("Disease")]
        [DisplayName("Disease Name")]
        public int DiseaseId { get; set; }
        public virtual Disease Disease { get; set; }
        public List<DiseaseTreatment> DiseaseTreatments { get; set; }
    }
}
