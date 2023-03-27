namespace EvergreenAPI.DTO
{
    public class ExtractDetectionHistoriesDTO
    {
        public int DetectionHistoryId { get; set; }
        public string ImageName { get; set; }
        public string DetectedDisease { get; set; }
        public double Accuracy { get; set; }
        public string ImageUrl { get; set; }
    }
}
