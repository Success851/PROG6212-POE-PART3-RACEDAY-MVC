namespace RaceDayMvc.Models
{
    public class CategoryViewModel
    {
        public int EventID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryType { get; set; } = "Distance";

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public int? DistanceKm { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
