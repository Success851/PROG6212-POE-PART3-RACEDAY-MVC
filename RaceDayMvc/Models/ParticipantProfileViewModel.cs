namespace RaceDayMvc.Models
{
    public class ParticipantProfileViewModel
    {
        public int ParticipantID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";

        public DateTime? DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
