using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class DiseaseMedicine
    {
        [Key]
        public int DiseaseMedicineId { get; set; }

        [ForeignKey("Disease")]
        public int DiseaseId { get; set; }
        [ForeignKey("Medicine")]
        public int? MedicineId { get; set; }
        public virtual Disease Disease { get; set; }
        public virtual Medicine Medicine { get; set; }
        
    }
}
