using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvergreenAPI.Models
{
    public class PlantCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PlantCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Plant> Plants { get; set; }
    }
}
