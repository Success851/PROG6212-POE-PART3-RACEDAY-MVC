namespace RaceDayMvc.Models
{
    public class OrganiserEnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public string Status { get; set; } = "";

        public OrganiserParticipantViewModel Participant { get; set; } = new();

        public OrganiserCategoryViewModel Category { get; set; } = new();
    }

    public class OrganiserParticipantViewModel
    {
        public int ParticipantID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";
    }

    public class OrganiserCategoryViewModel
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public string CategoryType { get; set; } = "";
    }
}
