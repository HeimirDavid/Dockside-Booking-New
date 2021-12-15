using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.ViewModels
{
    public class InfoForUpdatePrice
    {
        [Required]
        [DisplayName("BokningsNr")]
        public int BookingId { get; set; }
        [DisplayName("Förnamn")]
        public string UserFirstName { get; set; }
        [DisplayName("Efternamn")]
        public string UserLastName { get; set; }
        [DisplayName("Företagsnamn")]
        public string CompanyName { get; set; }
        [DisplayName("Startdatum")]
        public DateTime StartDate { get; set; }
        [DisplayName("Slutdatum")]
        public DateTime EndDate { get; set; }
        [Range(0, double.MaxValue)]
        [DisplayName("Pris")]
        public double Price { get; set; }
       
    }
}
