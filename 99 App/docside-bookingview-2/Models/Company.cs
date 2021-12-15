using docside_bookingview_2.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Företagsnamn måste anges")]
        [DisplayName("Företagsnamn")]
        public string strCompanyName { get; set; }
        [DisplayName("Rabatt")]
        //public decimal dblDiscount { get; set; }
        public int dblDiscount { get; set; }
        public ICollection<User> Users { get; set; }

        [Required(ErrorMessage = "Organisationsnummer måste anges")]
        [RegularExpression(@"([5]\d{5}[-]\d{4})|([5]\d{9}\s)", ErrorMessage="Felaktigt organisationsnummer! Försök igen")]
        [DisplayName("Organisationsnummer")]
        public string OrganisationsNummer { get; set; }

        [DisplayName("Faktureringsemail")]
        [Required(ErrorMessage = "Faktureringsemail måste anges")]
        [EmailAddress]
        public string CompanyEmail { get; set; }

        [DisplayName("Aktiv")]
        public bool Active { get; set; } = true;

    }
}
