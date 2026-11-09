using System.ComponentModel.DataAnnotations;

namespace RaceDayMvc.Models
{
    public class CreateResultViewModel
    {
        public int EnrolmentID { get; set; }

        [Required(ErrorMessage = "Please enter the finishing time.")]
        public TimeSpan? FinishTime { get; set; }

        public int? Position { get; set; }

        [Required(ErrorMessage = "Please select a result status.")]
        public string ResultStatus { get; set; } = "Completed";
    }
}