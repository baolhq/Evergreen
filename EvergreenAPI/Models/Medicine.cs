using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Medicine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Uses { get; set; }
        public List<Disease> Diseases { get; set; }

        [ForeignKey("MedicineCategories")]
        public int MedicineCategoryId { get; set; }
        public virtual MedicineCategory MedicineCategory { get; set; }
    }
}
