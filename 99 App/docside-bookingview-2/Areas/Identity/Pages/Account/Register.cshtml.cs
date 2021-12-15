using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using docside_bookingview_2.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace docside_bookingview_2.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "admin")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        //Added by Heimir
        private readonly ApplicationDbContext _db;



        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            //added by heimir
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _emailSender = emailSender;
            //Added by Heimir
            _db = db;
        }

        //Wierd attempts to fixing the issue with companies and user, start here
        //private IEnumerable<Company> companies;


        //public IEnumerable<Company> Companies
        //{
        //    get { return companies; }
        //    set { companies = _db.Companies; }
        //}
        //End here

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        //Added by heimir
        public Company Company { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public List<SelectListItem> Companies { get; set; }



        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Förnamn")]
            public string FirstName { get; set; }
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Efternamn")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Lösenord")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Bekräfta Lösenord")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            //Added by Heimir
            [Display(Name = "Company")]
            public int CompanyId { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Användarens roll")]
            public string UserRole { get; set; }

            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Telefonnummer")]
            public string TelephoneNumber { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!await _roleManager.RoleExistsAsync("internKund"))
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
                await _roleManager.CreateAsync(new IdentityRole("internKund"));
                await _roleManager.CreateAsync(new IdentityRole("externKund"));
            }

            //_userManager.AddToRoleAsync(user, "decompany");
            var activeCompanies = _db.Companies.Where(c => c.Active);

            Companies = activeCompanies.Select(a =>
                new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.strCompanyName
                }).ToList();




            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //var user = new User { UserName = Input.Email, Email = Input.Email };
                Company companyForUser = null;

                if (Input.CompanyId == -1)
                {
                    companyForUser = null;
                }
                else
                {
                    companyForUser = _db.Companies.Find(Input.CompanyId);
                }

                var user = new User { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName, Company = companyForUser, PhoneNumber = Input.TelephoneNumber, ActiveUser = true };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Input.UserRole);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //Comment out to not log in user straight away on time of registration
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "User");
                        //return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
