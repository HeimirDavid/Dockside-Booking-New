using docside_bookingview_2.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Bokningsstart")]
        public DateTime datStartBooking { get; set; }
        [Required]
        [DisplayName("Bokningsslut")]
        public DateTime datEndBooking { get; set; }

        public DateTime datTimeOfBooking { get; set; }
        public double dblBookingPrice { get; set; }
        public User User { get; set; }
        public Room Room { get; set; }
    }
}
