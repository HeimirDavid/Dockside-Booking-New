using docside_bookingview_2.Areas.Identity.Data;
using docside_bookingview_2.Data;
using docside_bookingview_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace docside_bookingview_2.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment hostEnvironment;

        public RoomsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this.hostEnvironment = hostEnvironment;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            IEnumerable<Room> roomList = _db.Rooms;

            return View(roomList);
        }

        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 
        //EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT - EDIT 

        // GET: Rum/Edit/id
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditRooms(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        //public async Task<IActionResult> EditRooms(int id, [Bind("Id,strRoomName,dcmWholeDay,dcmHalfDay,dcmHour,dcmInternalDiscount, SquareMetres, Available, Bookings,ImageTitle,ImageName,ImageFile,Floor,MaxPeople")] Room room)
        public async Task<IActionResult> EditRooms(int id, Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (room.ImageFile != null)
                    {
                        string wwwRootPath = hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(room.ImageFile.FileName);
                        string extension = Path.GetExtension(room.ImageFile.FileName);
                        room.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await room.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    _db.Update(room);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            return View(room);
        }

        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        //CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE - CREATE 
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        //GET-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult Create(Room obj)
        public async Task<IActionResult> Create([Bind("strRoomName,dcmWholeDay,dcmHalfDay,dcmHour,dcmInternalDiscount,SquareMetres,Available,ImageTitle,ImageName,ImageFile,Floor,MaxPeople")] Room room)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(room.ImageFile.FileName);
                string extension = Path.GetExtension(room.ImageFile.FileName);
                room.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await room.ImageFile.CopyToAsync(fileStream);
                }
                //Inserts record
                _db.Add(room);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(room);
        }

        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS
        //DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS - DETAILS

        // GET: Room/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        //DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN 
        //DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN 
        //DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN 
        //DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN - DETAILSADMIN 


        // GET: Room/Details/5
        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE
        //DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE - DELETE

        // GET: Room/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _db.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Room/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _db.Rooms.FindAsync(id);
            _db.Rooms.Remove(room);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================
        //====================================================================================================

        private bool RoomExists(int id)
        {
            return _db.Rooms.Any(e => e.Id == id);
        }
    }
}
