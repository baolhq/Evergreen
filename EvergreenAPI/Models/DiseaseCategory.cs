using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class DiseaseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiseaseCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
