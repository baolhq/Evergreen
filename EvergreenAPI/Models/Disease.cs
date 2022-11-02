using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Disease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiseaseId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Identification { get; set; }
        public string Affect { get; set; }
        public List<Medicine> Medicines { get; set; }
        public List<Treatment> Treatments { get; set; }
        [ForeignKey("DiseaseCategories")]
        [DisplayName("Disease Category")]
        public int DiseaseCategoryId { get; set; }
        public virtual DiseaseCategory DiseaseCategory { get; set; }

        [ForeignKey("Images")]
        [DisplayName("Image Description")]
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
    }
}
