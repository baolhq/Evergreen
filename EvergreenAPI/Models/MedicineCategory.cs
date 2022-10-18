using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class MedicineCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicineCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Medicine> Medicines { get; set; }
    }
}
