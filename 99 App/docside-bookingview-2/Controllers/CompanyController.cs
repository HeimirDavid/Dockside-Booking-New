using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using docside_bookingview_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Controllers
{
    public class CompanyController : Controller
    {
        // We need to import our DbContext. This contains all the information  from our Database
        private readonly ApplicationDbContext _db;

        public CompanyController(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// This Index method simply shows an entire list of all the companies in the system
        /// A part of admin pages. We land here when we choose "Visa" - "Företag" in admin sidebar
        /// </summary>
        /// <returns> companyList<Company> </returns>

        public IActionResult Index()
        {
            IEnumerable<Company> companyList = _db.Companies;

            return View(companyList);
        }

        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 

        /// <summary>
        /// GET: url: Company/EditCompanies/id
        /// Admin page: Visa - Företag - Edit
        /// This page show the inputs for editing an existing Company object
        /// </summary>
        /// <param name="id" (of Company)></param>
        /// <returns> 1 Company object to edit </returns>


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditCompanies(int? id)
        {
            // If no Company Id is sent with http request error 404 is returned
            if (id == null)
            {
                return NotFound();
            }

            // Find the relevant company according to the Company.Id sent in http request...
            // ... and return the Company object info to the view
            var company = await _db.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        /// <summary>
        /// POST: url: Company/EditCompanies/id
        /// Admin page: Visa - Företag - Edit
        /// When the Company input fields have been updated and the "Uppdatera" button is pressed
        /// </summary>
        /// <param name="id" (of Company) & name="company"></param>
        /// <returns> 1 Company object to edit </returns>

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        // public async Task<IActionResult> EditCompanies(int id, [Bind("Id,strCompanyName,dblDiscount,OrganisationsNummer,CompanyEmail,Active")] Company company)
        public async Task<IActionResult> EditCompanies(int id, Company company)
        {
            // If no Company Id is sent with http request error 404 is returned
            if (id != company.Id)
            {
                return NotFound();
            }

            // If the Company object is valid (ie. meets all decorator requirements - required, email format etc)
            // Then the databse is updated with the newly edited information 
            // and the user is sent back to the list of Companies (Company/Index)
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(company);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 

        /// <summary>
        /// GET: url: Company/Create
        /// Admin page: Lägg till - Företag
        /// Shows the necessary input fields for updating a Company object
        /// </summary>


        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// POST: url: Company/Create
        /// Admin page: Lägg till - Företag
        /// When the Company input fields have been filled in and the "Skapa" button is pressed
        /// This method fires. Saving the new Company to the database
        /// User is the returned to the list of Companies (Compnay/Index)
        /// </summary>
        /// <param name="company"</param>

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Create(Company obj)
        {
            var company = obj;
            _db.Companies.Add(company);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS

        /// <summary>
        /// GET: url: Company/Details/id
        /// Admin page: Visa - Företag - Details (icon)
        /// Shows all properties for the chosen Company object
        /// </summary>
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Return the Company object with the id sent from http request back to the view
            var company = await _db.Companies
                .FirstOrDefaultAsync(r => r.Id == id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        //HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY
        //HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY
        //HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY
        //HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY - HISTORY

        /// <summary>
        /// GET: url: Company/History/id
        /// Admin page: Visa - Företag - History (icon)
        /// Shows all bookings for the chosen Company object
        /// </summary>

        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> History(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find the Company with the id sent from http request...
            // ... and add the User connected to that company
            var company = await _db.Companies.Include(p => p.Users)
                .FirstOrDefaultAsync(r => r.Id == id);

            // A simple Viewbag used to display the companys name in the front end view
            ViewBag.CompanyName = company.strCompanyName;

            var users = company.Users.ToList();
            var allUsers = _db.Users.Include(b => b.Bookings).ThenInclude(c => c.Room);
            var bookings = new List<Booking>();
            var cUsersWithBookings = new List<User>();

            // Creates a list of all Company users with a booking! (cUsersWithBookings)
            // For every user in the chosen Company...
            // Check to see if that user has a booking...
            // and add it to the list named "cUserWithBookings"
            foreach (var cUser in users)
            {
                foreach (var user in allUsers)
                {
                    if (cUser == user)
                    {
                        cUsersWithBookings.Add(user);
                    }
                }
            }

            // Creates a list of all Company users with a booking...(bookings)
            // BEFORE DateTime.Now (eg. History)
            foreach (var user in cUsersWithBookings)
            {
                var userBookings = user.Bookings;


                foreach (var booking in userBookings)
                {

                    if (DateTime.Now > booking.datEndBooking)
                    {
                        bookings.Add(booking);
                    }
                }
            }

            if (company == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE

        // GET: Company/Delete/id
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _db.Companies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Company/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _db.Companies.FindAsync(id);
            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================

        private bool CompanyExists(int id)
        {
            return _db.Companies.Any(e => e.Id == id);
        }

    }
}
