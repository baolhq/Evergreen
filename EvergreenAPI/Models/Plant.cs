using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class Plant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlantId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Image> Images { get; set; }
        [ForeignKey("PlantCategories")]
        [DisplayName("Plant Category")]
        public int PlantCategoryId { get; set; }
        public virtual PlantCategory PlantCategory { get; set; }
    }
}
