using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using docside_bookingview_2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Controllers
{
    public class UserController : Controller
    {
        // ///////////////////////////////////////////////
        // Added by Richard to enable list of users
        //private ApplicationDbContext _app;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public UserController(UserManager<User> userManager, ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        // ///////////////////////////////////////////////
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            //var users = _userManager.Users;
            var users = _db.Users.Include(u => u.Company).ToList();

            var usersWithCompany = new List<UserWithCompanyName>();

            foreach (var user in users)
            {
               
               // var company = _db.Companies.Find(user.Company);
                if (user.Company == null)
                    {
                        usersWithCompany.Add(
                        new UserWithCompanyName
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            CompanyName = null,
                            PhoneNumber = user.PhoneNumber,
                            Active=user.Active,
                        }
                    );
                } 
                else
                {
                    usersWithCompany.Add(
                    new UserWithCompanyName
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            CompanyName = user.Company.strCompanyName,
                            PhoneNumber = user.PhoneNumber,
                            Active=user.Active,
                        }
                    );
                }
                
            };
            

            return View(usersWithCompany);
        }


        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 

        // GET: Rum/User/id
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUsers(string id)
        {
            ViewBag.Companies = _db.Companies.ToList();
            if (id == null)
            {
                return NotFound();
            }

            User user = await _db.Users.FindAsync(id);
            var allUserRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var userWithRole = allUserRoles.Find(ur => ur.UserId == id);
            var userRoleName = _db.Roles.Find(userWithRole.RoleId);

            var rolesList = new List<string>();

            for (int i = 0; i < roles.Count; i++)
            {
                rolesList.Add(roles[i].Name);
            }

            ViewBag.Roles = rolesList;
            ViewBag.UserRoleName = userRoleName;

            //roles[0].Name


            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditUsers(string user_role, string id, int? CompanyId, [Bind("Id,FirstName,LastName,Email,Company,PhoneNumber,Active")] User userFromHTTP)
        {

            //if (id != user.Id)
            //User id is a guid....so I had to change to string.equals
            if (!string.Equals(id, userFromHTTP.Id))
            {
                return NotFound();
            }



            if (ModelState.IsValid)
            {
                Company ChangedCompany = _db.Companies.Find(CompanyId);

                try
                {
                    // Updating the db user object with the info from the http request (edit form)
                    User userFromDB = await _db.Users.FindAsync(id);
                    userFromDB.FirstName = userFromHTTP.FirstName;
                    userFromDB.LastName = userFromHTTP.LastName;
                    userFromDB.Email = userFromHTTP.Email;
                    userFromDB.PhoneNumber = userFromHTTP.PhoneNumber;
                    userFromDB.Company = ChangedCompany;
                    userFromDB.Active = userFromHTTP.Active;

                    //Edit user roles
                    //Get all the different object of UserRoles - contains RoleId and UserId
                    var allUserRoles = _db.UserRoles.ToList();

                    //Get the new role from Http request
                    var newRole = _db.Roles.Where(r => r.Name == user_role).ToList();
                    
                    // This is the object that contains the RoleId that we wish to update. To update it it needs to be deleted
                    var userRolesObj = allUserRoles.Find(ur => ur.UserId == id);

                    // Foreign keys can't just be updated. We must remove the old object and then create a new.
                    _db.Remove(userRolesObj);

                    //here we create a new object to go in to the table 'UserRoles' and add it to the database
                    _db.UserRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = newRole[0].Id,
                        UserId = userFromDB.Id,
                    });
                   
                    _db.Update(userFromDB);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userFromHTTP.Id))
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
            return View(userFromHTTP);
        }

        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS

        // GET: User/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users.Include(u => u.Company)
                .FirstOrDefaultAsync(u => u.Id == id);

           

            var company = _db.Companies.Find(user.Company.Id);

            UserWithCompanyName userWithCompany = new UserWithCompanyName();

            if (company == null)
            {
                userWithCompany = new UserWithCompanyName
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CompanyName = null,
                    PhoneNumber = user.PhoneNumber,
                    Active = user.Active,
                };
            } else
            {
                userWithCompany = new UserWithCompanyName
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CompanyName = company.strCompanyName,
                    PhoneNumber = user.PhoneNumber,
                    Active = user.Active,
                };
            }
            
            if (user == null)
            {
                return NotFound();
            }

            return View(userWithCompany);
        }

        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE

        // GET: User/Delete/id
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _db.Users.FindAsync(id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================

        //// User id is a guid.... so I had to change to from int to string
        private bool UserExists(string id)
        {
            return _db.Users.Any(e => e.Id == id);
        }

        
    }
}
