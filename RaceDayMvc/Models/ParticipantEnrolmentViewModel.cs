
namespace RaceDayMvc.Models
{
    public class ParticipantEnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string Status { get; set; } = "";

        public EnrolledEventViewModel Event { get; set; } = new();

        public EnrolledCategoryViewModel Category { get; set; } = new();
    }

    public class EnrolledEventViewModel
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public DateTime EventDate { get; set; }

        public string Location { get; set; } = "";
    }

    public class EnrolledCategoryViewModel
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public string CategoryType { get; set; } = "";
    }
}

