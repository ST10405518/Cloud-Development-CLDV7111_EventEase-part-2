using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required")]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public Venue? Venue { get; set; }

        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Customer Name")]
        public string? CustomerName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Customer Email")]
        public string? CustomerEmail { get; set; }

        [StringLength(20)]
        [Display(Name = "Customer Phone")]
        public string? CustomerPhone { get; set; }

        [StringLength(20)]
        [Display(Name = "Booking Status")]
        public string BookingStatus { get; set; } = "Confirmed";

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
