using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Areas.Identity.Pages.Account;
using docside_bookingview_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.ViewModels
{
    public class RegisterUserWithCompany
    {
        public RegisterModel RegisterModel { get; set; }
        public IEnumerable<Company> Companies { get; set; }

        public string ReturnUrl { get; set; }

    }
}
