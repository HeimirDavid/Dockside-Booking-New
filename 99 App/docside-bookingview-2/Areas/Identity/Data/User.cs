using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using docside_bookingview_2.Models;
using Microsoft.AspNetCore.Identity;

namespace docside_bookingview_2.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Förnamn")]
        public string FirstName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Efternamn")]
        public string LastName { get; set; }

        public ICollection<Booking> Bookings { get; set; }


        //[DisplayName("Bolagsid")]
        //public int? CompanyId { get; set; }
        [DisplayName("Företag")]
        public Company Company { get; set; }

        [DisplayName("Aktiv användare")]
        public bool ActiveUser { get; set; } = true;

        //[DisplayName("Telefonnummer")] 
        //public string TelephoneNumber { get; set; }

        [DisplayName("Aktiv")]
        public bool Active { get; set; } = true;
    }
}
