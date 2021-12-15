using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using docside_bookingview_2.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace docside_bookingview_2.Controllers
{
    public class BookingController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<User> _userManager;

        public BookingController(ApplicationDbContext db, UserManager<User> userManager)
        {
            _userManager = userManager;
            _db = db;
        }


        //INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX 
        //INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX 
        //INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX - INDEX 

        public IActionResult Index()
        {

            IEnumerable<Room> objList = _db.Rooms;
            return View(objList);
        }


        //GET        
        //public IActionResult Create(int roomId)
        //[Authorize(Roles = "Admin")]
        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {

            var room = await _db.Rooms.FindAsync(id);
            var bookings = _db.Bookings.Include(b => b.Room).ToList();
            var users = _db.Users.ToList();
            var existingBookingsForRoom = bookings.Where(p => p.Room?.Id == id).ToList();
            var roomBookingsForFullCalendar = new List<BookingForRoom>();


            foreach (var booking in existingBookingsForRoom)
            {
                roomBookingsForFullCalendar.Add(new BookingForRoom
                {
                    Title = "Bokat",
                    Start = booking.datStartBooking,
                    End = booking.datEndBooking,
                }
                );

            };


            var json = JsonSerializer.Serialize(roomBookingsForFullCalendar);

            ViewBag.BookingDates = json;

            RoomBooking roomBooking = new RoomBooking
            {
                Room = room,
                Users = users,
            };

            return View(roomBooking);
        }


        //POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE 
        //POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE 
        //POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE 
        //POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE - POSTCREATE 

        //POST
        //public async Task<IActionResult> CreatePost(int? id, Booking obj)
        [HttpPost]
        public IActionResult CreatePost(RoomBooking obj, string userChosenByAdmin)
        {
            User correctUser = null;
            string userId = null;
            var allUsersWithCompany = _db.Users.Include(u => u.Company).ToList();
            var room = _db.Rooms.Find(obj.Room.Id);
            //var allUsersWithCompany = _db.Users.Include(u => u.Company);
            //allUsersWithCompany = allUsersWithCompany.Include()

            //('t => ((Derived)t).MyProperty')


            int discount = 0;

            if (User.IsInRole("admin"))
            {
                correctUser = allUsersWithCompany.Find(u => u.Id == userChosenByAdmin);
                var allUserRoles = _db.UserRoles.ToList();

                var userWithRole = allUserRoles.Find(ur => ur.UserId == userChosenByAdmin);
                var userRoleName = _db.Roles.Find(userWithRole.RoleId);

                if (userRoleName.Name == "internKund" && !room.strRoomName.Contains("Flex"))
                {
                    discount = room.dcmInternalDiscount;
                }
            }
            else
            {
                if (User.IsInRole("internKund") && !room.strRoomName.Contains("Flex"))
                {
                    discount = room.dcmInternalDiscount;
                }
                userId = _userManager.GetUserId(HttpContext.User);
                correctUser = allUsersWithCompany.Find(u => u.Id == userId);
            }


            //if (correctUser.IsInRole("internKund"))
            //{
            //    discount = correctUser.Company.dblDiscount;
            //}




            

            if (correctUser == null)
            {
                TempData["Message"] = "Du måste vara inloggad för att boka ett rum";
                return RedirectToAction("Index");
            }
            else if (room == null)
            {
                TempData["Message"] = "Du måste välja ett rum att boka";
                return RedirectToAction("Index");
            }

            ///// Maybe old shit?
            var bookings = _db.Bookings.Include(b => b.Room).ToList();
            var bookingsToRoom = bookings.Where(p => p.Room?.Id == room.Id).ToList();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            var totalPrice = GetBestPrice(obj.Booking.datStartBooking, obj.Booking.datEndBooking, room);

            double discountedTotalPrice = (double)totalPrice - ((double)totalPrice * ((double)discount / 100));

            //var newTotalPrice = totalPrice - 100;






            Booking booking = new Booking
            {
                datStartBooking = obj.Booking.datStartBooking,
                datEndBooking = obj.Booking.datEndBooking,
                datTimeOfBooking = DateTime.Now,
                dblBookingPrice = discountedTotalPrice,
                User = correctUser,
                Room = room,

            };

            _db.Bookings.Add(booking);
            _db.SaveChanges();
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("AdminBookingList");
            }
            else
            {
                return RedirectToAction("UserBookings");
            }
        }


        //SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH 
        //SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH 
        //SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH 
        //SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH - SEARCH 


        [HttpPost]
        public IActionResult SearchBooking(DateTime? startTime, DateTime? endTime)
        {

            if (startTime == null || endTime == null)
            {
                TempData["Message"] = "Du måste välja både ett startdatum och ett slutdatum";
                return RedirectToAction("Index");
            }
            //IEnumerable<Room> objList = _db.Rooms;
            //var bookings = _db.Bookings;

            //var bookings = _db.Bookings.Include(b => b.Room).ToList();

            var rooms = _db.Rooms.Include(b => b.Bookings).ToList();



            //var availibleRooms = new List<Room>();
            var unavailibleRooms = new List<Room>();
            //Start date is before enddate
            //startdate < enddate

            foreach (var room in rooms)
            {
                foreach (var booking in room.Bookings)
                {
                    //(targetDt.Ticks > d1.Ticks && targetDt.Ticks < d2.Ticks)
                    if (startTime > booking.datStartBooking && startTime < booking.datEndBooking)
                    {
                        unavailibleRooms.Add(room);
                    }
                    else if (endTime > booking.datStartBooking && endTime < booking.datEndBooking)
                    {
                        unavailibleRooms.Add(room);
                    }
                    else if (startTime < booking.datStartBooking && endTime > booking.datEndBooking)
                    {
                        unavailibleRooms.Add(room);
                    }

                }
                if (!room.Available)
                {
                    unavailibleRooms.Add(room);
                }
            }

            foreach (var item in unavailibleRooms)
            {
                rooms.Remove(item);
            }


            return View(rooms);
        }

        //ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST 
        //ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST 
        //ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST 
        //ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST - ADMINBOOKINGLIST 

        [Authorize(Roles = "admin")]
        public IActionResult AdminBookingList()
        {
            var bookings = _db.Bookings.Include(b => b.Room);

            var bookingsWithUsers = bookings.Include(u => u.User).ThenInclude(u => u.Company).ToList();
            

            return View(bookingsWithUsers);
        }



        //// ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET
        //// ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET
        //// ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET
        //// ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT -GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET - ADMIN-BOOKING-EDIT-GET

        [Authorize(Roles = "admin")]
        public IActionResult AdminBookingEdit(int id)
        {

           var singleBookingWithUser = _db.Bookings.Include(b => b.User).FirstOrDefault(b => b.Id == id);

            var bookingToEdit = _db.Bookings.Include(b => b.Room).FirstOrDefault(b => b.Id == id);
           


            var room = bookingToEdit.Room;

            var bookings = _db.Bookings.Include(b => b.Room).ToList();

            //var bookingToEditWithRoom = bookings.Where(b => b.Id == id);

            var users = _db.Users.ToList();
            var existingBookingsForRoom = bookings.Where(p => p.Room?.Id == room.Id).ToList();
            var roomBookingsForFullCalendar = new List<BookingForRoom>();


            foreach (var booking in existingBookingsForRoom)
            {
                if (booking.Id == id)
                {
                    continue;
                }
                else
                {
                    roomBookingsForFullCalendar.Add(new BookingForRoom
                    {
                        Title = "Bokat",
                        Start = booking.datStartBooking,
                        End = booking.datEndBooking,
                    });
                }
            };


            var json = JsonSerializer.Serialize(roomBookingsForFullCalendar);

            ViewBag.BookingToEdit = bookingToEdit;
            ViewBag.BookingDates = json;
            ViewBag.singleBookingWithUser = singleBookingWithUser;

            RoomBooking roomBooking = new RoomBooking
            {
                Room = room,
                Users = users,
            };

            return View(roomBooking);
        }


        // ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST -
        // ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST -
        // ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST -
        // ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST - ADMIN-BOOKING-EDIT-POST -

        //public async Task<IActionResult> AdminBookingEdit(int id, [Bind("Id,datStartBooking,datEndBooking,datTimeOfBooking,dblBookingPrice,User,Room")] Booking bookingFromPost)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminBookingEdit(int id, RoomBooking bookingFromPost)
        {
            var allBookingsFromDB = _db.Bookings.Include(b => b.User).ToList();

            var bookingFromDB = allBookingsFromDB.Find(b => b.Id == id);
            var roomFromDB = await _db.Rooms.FindAsync(bookingFromPost.Room.Id);

            bookingFromDB.datStartBooking = bookingFromPost.Booking.datStartBooking;
            bookingFromDB.datEndBooking = bookingFromPost.Booking.datEndBooking;

            int discount = 0;

            if (User.IsInRole("admin"))
            {
                // correctUser = allUsersWithCompany.Find(u => u.Id == userChosenByAdmin);
                var userChosenByAdmin = bookingFromDB.User.Id;
                var allUserRoles = _db.UserRoles.ToList();

                var userWithRole = allUserRoles.Find(ur => ur.UserId == userChosenByAdmin);
                var userRoleName = _db.Roles.Find(userWithRole.RoleId);

                if (userRoleName.Name == "internKund" && !roomFromDB.strRoomName.Contains("Flex"))
                {
                    discount = roomFromDB.dcmInternalDiscount;
                }
            }

            var totalPrice = GetBestPrice(bookingFromPost.Booking.datStartBooking, bookingFromPost.Booking.datEndBooking, roomFromDB);
            double discountedTotalPrice = (double)totalPrice - ((double)totalPrice * ((double)discount / 100));
            bookingFromDB.dblBookingPrice = discountedTotalPrice;

            //bookingFromDB.dblBookingPrice = GetBestPrice(bookingFromPost.Booking.datStartBooking, bookingFromPost.Booking.datEndBooking, roomFromDB);
            if (bookingFromPost.Booking.User != null)
            {
                bookingFromDB.User = bookingFromPost.Booking.User;
            }


            if (bookingFromDB != null)
            {
                //Code
                try
                {
                    _db.Update(bookingFromDB);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (bookingFromPost == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminBookingList));
            }
            return RedirectToAction(nameof(AdminBookingList));
        }

        // UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin
        // UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin
        // UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin
        // UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin - UpdateBookingPrice Admin
        [Authorize(Roles = "admin")]
        public IActionResult UpdateBookingPrice(int id)
        {
            var allBookings = _db.Bookings.Include(b => b.User).ThenInclude(u => u.Company).ToList();

            var booking = allBookings.Find(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingInfo = new InfoForUpdatePrice
            {
                BookingId = booking.Id,
                UserFirstName = booking.User.FirstName,
                UserLastName = booking.User.LastName,
                CompanyName = booking.User.Company.strCompanyName,
                StartDate = booking.datStartBooking,
                EndDate = booking.datEndBooking,
                Price = booking.dblBookingPrice,
            };
            
            return View(bookingInfo);
        }



        // UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - 
        // UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - 
        // UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - 
        // UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - UpdateBookingPrice Admin POST - 
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult UpdateBookingPrice(double special_price, int booking_id)
        {
            var bookingFromDB = _db.Bookings.Find(booking_id);
            bookingFromDB.dblBookingPrice = special_price;


            if (ModelState.IsValid)
            {
                _db.Update(bookingFromDB);
                _db.SaveChanges();
            } else
            {
                return NotFound();
            }
           

            return RedirectToAction("AdminBookingList");
        }

            //public int BookingId { get; set; }
            //public string UserFirstName { get; set; }
            //public string UserLastName { get; set; }

            //public string CompanyName { get; set; }
            //public DateTime StartDate { get; set; }
            //public DateTime EndDate { get; set; }
            //public int Price { get; set; }


            // Delete GET -  Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET -
            // Delete GET -  Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET -
            // Delete GET -  Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET -
            // Delete GET -  Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET - Delete GET -
            [Authorize(Roles = "admin")]
        public IActionResult AdminBookingDelete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var booking = _db.Bookings.Include()
            //   .FirstOrDefault(b => b.Id == id);
            //var bookingWithRoom = booking
            var bookingsWithUser = _db.Bookings.Include(b => b.User);
            var bookingWithUserAndRoom = bookingsWithUser.Include(r => r.Room);
            var booking = bookingWithUserAndRoom.Where(b => b.Id == id).ToList();
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST - 
        // Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST - 
        // Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST - 
        // Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST -  Delete POST - 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var booking = await _db.Bookings.FindAsync(id);
            _db.Bookings.Remove(booking);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(AdminBookingList));
        }



        // USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR -
        // USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR -
        // USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR -
        // USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR - USERBOOKINGS / MINA BOOKNINGAR -

        [Authorize]
        public IActionResult UserBookings()
        {

            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId == null)
            {
                return NotFound();
            }

            var bookingsWithUsers = _db.Bookings.Include(b => b.User);
            var bookingsWithUsersAndRooms = bookingsWithUsers.Include(b => b.Room).ToList();

            var bookingsToUser = bookingsWithUsersAndRooms.Where(p => p.User.Id == userId).ToList();

            var comingBookings = new List<Booking>();
            var previousBookings = new List<Booking>();

            foreach (var booking in bookingsToUser)
            {
                if (booking.datStartBooking > DateTime.Now)
                {
                    comingBookings.Add(booking);
                }
                else
                {
                    previousBookings.Add(booking);
                }
            }

            ViewBag.ComingBookings = comingBookings;
            ViewBag.PreviousBookings = previousBookings;
            //var userWithBoookings = user.Bookings;

            //var bookings = _db.Bookings.Include(b => b.Room);
            //var bookingsWithUsers = bookings.Include(u => u.User).ToList();

            return View();
        }

        //BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET 
        //BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET 
        //BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET 
        //BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET - BOOKINGEDIT GET 
        [Authorize]
        public IActionResult BookingEdit(int id)
        {

            var bookingToEdit = _db.Bookings.Include(b => b.Room).FirstOrDefault(b => b.Id == id);
            var room = bookingToEdit.Room;
            var bookings = _db.Bookings.Include(b => b.Room).ToList();

            var users = _db.Users.ToList();
            var existingBookingsForRoom = bookings.Where(p => p.Room?.Id == room.Id).ToList();
            var roomBookingsForFullCalendar = new List<BookingForRoom>();




            if (bookingToEdit.datStartBooking < DateTime.Now)
            {
                return RedirectToAction("UserBookings");
            }

            foreach (var booking in existingBookingsForRoom)
            {
                if (booking.Id == id)
                {
                    continue;
                }
                else
                {
                    roomBookingsForFullCalendar.Add(new BookingForRoom
                    {
                        Title = "Bokat",
                        Start = booking.datStartBooking,
                        End = booking.datEndBooking,
                    });
                }
            };


            var json = JsonSerializer.Serialize(roomBookingsForFullCalendar);

            ViewBag.BookingToEdit = bookingToEdit;
            ViewBag.BookingDates = json;

            RoomBooking roomBooking = new RoomBooking
            {
                Room = room,
                Users = users,
            };

            return View(roomBooking);
        }


        //BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - 
        //BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - 
        //BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - 
        //BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - BOOKINGEDIT POST - 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> BookingEdit(int id, RoomBooking bookingFromPost)
        {
            var bookingFromDB = await _db.Bookings.FindAsync(id);
            var roomFromDB = await _db.Rooms.FindAsync(bookingFromPost.Room.Id);

            bookingFromDB.datStartBooking = bookingFromPost.Booking.datStartBooking;
            bookingFromDB.datEndBooking = bookingFromPost.Booking.datEndBooking;

            int discount = 0;

            if (User.IsInRole("internKund") && !roomFromDB.strRoomName.Contains("Flex"))
            {
                discount = roomFromDB.dcmInternalDiscount;
            }

            //userId = _userManager.GetUserId(HttpContext.User);
            //correctUser = allUsersWithCompany.Find(u => u.Id == userId);

            //bookingFromDB.dblBookingPrice = GetBestPrice(bookingFromPost.Booking.datStartBooking, bookingFromPost.Booking.datEndBooking, roomFromDB);
            //var price = GetBestPrice(bookingFromPost.Booking.datStartBooking, bookingFromPost.Booking.datEndBooking, roomFromDB);

            var totalPrice = GetBestPrice(bookingFromPost.Booking.datStartBooking, bookingFromPost.Booking.datEndBooking, roomFromDB);
            double discountedTotalPrice = (double)totalPrice - ((double)totalPrice * ((double)discount / 100));
            bookingFromDB.dblBookingPrice = discountedTotalPrice;
            //if (bookingFromPost.Booking.User != null)
            //{
            //    bookingFromDB.User = bookingFromPost.Booking.User;
            //}


            if (bookingFromDB != null)
            {
                //Code
                try
                {
                    _db.Update(bookingFromDB);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (bookingFromPost == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UserBookings");
            }
            return RedirectToAction("UserBookings");
        }


        // GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - 
        // GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - 
        // GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - 
        // GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - GET BEST PRICE METHOD - 
        public int GetBestPrice(DateTime startTime, DateTime endTime, Room room)
        {
            // räkna fram det billigaste alternativet till kunden
            int numberOfWholeDays = (endTime.Day - startTime.Day) - 1;
            int numberOfDaysForFlex = (endTime.Day - startTime.Day) + 1;
            int hoursFirstDay = 0;

            double hoursFirstDayDBL = startTime.Hour;
            double hoursLastDayDBL = endTime.Hour;

            int hoursLastDay = 0;
            int totalPrice = 0;

            if (room.strRoomName.Contains("Flex"))
            {
                totalPrice = numberOfDaysForFlex * room.dcmWholeDay;
                return totalPrice;
            }

            if (numberOfWholeDays >= 1)
            {
                //hoursFirstDay = 19 - obj.Booking.datStartBooking.Hour;
                hoursFirstDay = (int)Math.Ceiling(19 - hoursFirstDayDBL);
                hoursLastDay = (int)Math.Ceiling(hoursLastDayDBL - 7);
                totalPrice = numberOfWholeDays * room.dcmWholeDay;

                if (hoursFirstDay >= 6 && hoursFirstDay < 12)
                {
                    int restHours = hoursFirstDay - 6;
                    totalPrice += room.dcmHalfDay;
                    totalPrice += restHours * room.dcmHour;
                }
                else if (hoursFirstDay == 12)
                {
                    totalPrice += room.dcmWholeDay;
                }
                else
                {
                    int restHours = hoursFirstDay;
                    totalPrice += restHours * room.dcmHour;
                }

                if (hoursLastDay >= 6 && hoursLastDay < 12)
                {
                    int restHours = hoursLastDay - 6;
                    totalPrice += room.dcmHalfDay;
                    totalPrice += restHours * room.dcmHour;
                }
                else if (hoursLastDay == 12)
                {
                    totalPrice += room.dcmWholeDay;
                }
                else
                {
                    int restHours = hoursLastDay;
                    totalPrice += restHours * room.dcmHour;
                }
            }
            if (numberOfWholeDays < 1)
            {
                TimeSpan timespan = (endTime - startTime);
                int totalHours = (int)Math.Ceiling(timespan.TotalHours);


                if (totalHours >= 6 && totalHours < 12)
                {
                    int restHours = totalHours - 6;
                    totalPrice += room.dcmHalfDay;
                    totalPrice += restHours * room.dcmHour;
                }
                else if (totalHours >= 12 && totalHours <= 24)
                {
                    totalPrice += room.dcmWholeDay;
                }
                else if (totalHours > 24)
                {
                    int restHours = totalHours - 24;
                    totalPrice += room.dcmWholeDay;
                    if (restHours < 6)
                    {
                        totalPrice += restHours * room.dcmHour;
                    }
                    else if (restHours >= 12)
                    {
                        totalPrice += room.dcmWholeDay;
                    }
                    else if (restHours > 6 && restHours < 12)
                    {
                        totalPrice += room.dcmHalfDay;
                        restHours -= 6;
                        totalPrice += restHours * room.dcmHour;
                    }
                    else
                    {
                        totalPrice += room.dcmHalfDay;
                    }
                }
                else
                {
                    totalPrice += totalHours * room.dcmHour;
                }
            }
            return totalPrice;
        }

    }
}
