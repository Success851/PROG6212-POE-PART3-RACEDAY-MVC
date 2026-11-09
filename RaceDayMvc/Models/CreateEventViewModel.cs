namespace RaceDayMvc.Models
{
    public class CreateEventViewModel
    {
        public string EventName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime EventDate { get; set; } = DateTime.Today.AddDays(7);

        public string Location { get; set; } = string.Empty;

        public int Distance { get; set; }

        public string EventType { get; set; } = string.Empty;
    }
}

