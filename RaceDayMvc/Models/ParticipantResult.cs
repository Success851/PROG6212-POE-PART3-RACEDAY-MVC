namespace RaceDayMvc.Models
{
    public class ParticipantResult
    {
        public int ResultID { get; set; }

        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        public string ResultStatus { get; set; } = "";

        public DateTime RecordedAt { get; set; }

        public ResultEvent Event { get; set; } = new ResultEvent();

        public ResultCategory Category { get; set; } =
            new ResultCategory();
    }

    public class ResultEvent
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = "";

        public string EventType { get; set; } = "";
    }

    public class ResultCategory
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public string CategoryType { get; set; } = "";
    }
}
