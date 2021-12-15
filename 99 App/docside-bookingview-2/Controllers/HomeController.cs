using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using docside_bookingview_2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace docside_bookingview_2.Controllers
{
    public class HomeController : Controller
    {

        // ///////////////////////////////////////////////
        // Added by Richard to enable list of users
        //private ApplicationDbContext _app;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public HomeController(UserManager<User> userManager, ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
        }
        // ///////////////////////////////////////////////

        

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {

            var bookings = _db.Bookings.Include(b => b.Room);
            var bookingsWithUsers = bookings.Include(b => b.User).ThenInclude(u => u.Company).ToList();


            var rooms = _db.Rooms.Include(b => b.Bookings).ToList();

      
            var ongoingBooking = new List<Room>();
            var availableRooms = new List<Room>();
            var ListOfRoomsForFrontEnd = new List<RoomAvailibleNow>();

            //Start date is before enddate
            //startdate < enddate
            //Bookings endtime is after current time and booking startime is before current time.
            foreach (var room in rooms)
            {
                foreach (var booking in room.Bookings)
                {
                    if (booking.datEndBooking > DateTime.Now && booking.datStartBooking < DateTime.Now)
                    {
                        ListOfRoomsForFrontEnd.Add(new RoomAvailibleNow
                                {
                                    RoomId = room.Id,
                                    RoomName = room.strRoomName,
                                    SquareMetres = room.SquareMetres,
                                    Floor = room.Floor,
                                    MaxPeople = room.MaxPeople,
                                    Available = false,
                                    Active = room.Available,
                                }
                        );
                    }
                }
            }


            foreach (var room in rooms)
            {
                bool containsItem = ListOfRoomsForFrontEnd.Any(item => item.RoomId == room.Id);
                if (!containsItem)
                {
                    ListOfRoomsForFrontEnd.Insert(0, new RoomAvailibleNow
                        {
                            RoomId = room.Id,
                            RoomName = room.strRoomName,
                            SquareMetres = room.SquareMetres,
                            Floor = room.Floor,
                            MaxPeople = room.MaxPeople,
                            Available = true,
                            Active = room.Available,
                    }
                    );
                }
            }
            if (User.IsInRole("admin"))
            {
                var TodaysBookings = new List<Booking>();
                var TomorrowsBookings = new List<Booking>();

                ViewBag.TodaysBookings = TodaysBookings;
                ViewBag.TomorrowsBookings = TomorrowsBookings;

                //Bookings endtime is today or after current date and booking startime is today or before current date.
                foreach (var booking in bookingsWithUsers)
                {
                    if (booking.datEndBooking.Date >= DateTime.Now.Date && booking.datStartBooking.Date <= DateTime.Now.Date)
                    {
                        TodaysBookings.Add(booking);
                    } 
                    
                }


                foreach (var booking in bookingsWithUsers)
                {
                    if (booking.datEndBooking.Date >= DateTime.Now.Date.AddDays(1) && booking.datStartBooking.Date <= DateTime.Now.Date.AddDays(1))
                    {
                        TomorrowsBookings.Add(booking);
                    }
                }


                    return View(bookingsWithUsers);
            } else
            {
                return View(ListOfRoomsForFrontEnd);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        // ///////////////////////////////////////////////
        // Added by Richard to enable list of users
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }
        // ///////////////////////////////////////////////

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
