using System.ComponentModel.DataAnnotations;

namespace EvergreenAPI.DTO
{
    public class PlantDTO
    {
        public int PlantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
