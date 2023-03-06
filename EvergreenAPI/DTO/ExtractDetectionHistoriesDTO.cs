namespace EvergreenAPI.DTO
{
    public class ExtractDetectionHistoriesDTO
    {
        public string ImageName { get; set; }
        public string DetectedDisease { get; set; }
        public float Accuracy { get; set; }
        public string ImageUrl { get; set; }
    }
}
