
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDayMvc.Models
{
    public class UpdateEventViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event name is required.")]
        public string EventName { get; set; } = "";

        public string Description { get; set; } = "";

        [Required]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = "";

        [Range(1, 1000, ErrorMessage = "Distance must be greater than zero.")]
        public decimal DistanceKm { get; set; }

        [Required(ErrorMessage = "Please select an event type.")]
        public string EventType { get; set; } = "";
    }
}

