namespace RaceDayMvc.Models
{
    public class Event
    {
        public int EventID { get; set; }

        public string EventName { get; set; }

        public string Description { get; set; }

        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public double DistanceKm { get; set; }

        public string EventType { get; set; }
    }
}
