namespace RaceDayMvc.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public int EventID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryType { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public double DistanceKm { get; set; }

        public string Description { get; set; }
    }
}
