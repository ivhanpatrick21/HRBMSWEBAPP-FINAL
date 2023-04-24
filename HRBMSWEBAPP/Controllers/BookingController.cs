﻿
using HRBMSWEBAPP.Data;
using HRBMSWEBAPP.Models;
using HRBMSWEBAPP.Repository;
using HRBMSWEBAPP.Service;
using HRBMSWEBAPP.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRBMSWEBAPP.Controllers
{
    //[Authorize(Roles ="Admin, User")]
    public class BookingController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IBookingDBRepository _repo;
       
        IRoomDBRepository _Roomrepo;
        private readonly IUserService _UserService;
        HRBMSDBCONTEXT _context;

        public BookingController(IBookingDBRepository repo, IRoomDBRepository Roomrepo, 
                                 HRBMSDBCONTEXT context, UserManager<ApplicationUser> userManager,
                                 IUserService userService)
        {
            this._repo = repo;
            _Roomrepo = Roomrepo;
            _context = context;
            _userManager = userManager;
            _UserService = userService;
        }

        //public IActionResult GetAllBookings()
        //{
        //    var booklist = _repo.GetAllBooking();
        //    return View(booklist);

        //}

        public async Task<IActionResult> GetAllBookings()
        {
            //var rooms = _context.Room.ToList();
            //var room = new Room { Status = true };

            //List<ApplicationUser> userlist = new List<ApplicationUser>();
            //userlist = _userManager.Users.ToList();
            //var availableDisplayString = room.DisplayStatus;
            //var bookedDisplayString = "Booked";

            List<Booking> booking = await this._repo.GetAllBooking();
            return View(booking);
        }

        //public async Task<IActionResult> GetAllBookings()
        //{

        //    List<Booking> booking = await this._repo.GetAllBooking();
        //    return View(booking);
        //}
        public async Task<IActionResult> Details(int? id)
        {


            if (id == null)
            {
                return NotFound();
            }

         
            Booking booking = await this._repo.GetBookingById((int)id);
            return View(booking);
        }


        //public IActionResult Details(int bookId)
        //{
        //    var book = _repo.GetBookingById(bookId);
        //    return View(book);
        //}

        //public IActionResult Delete(int id)
        //{
        //    var booklist = _repo.DeleteBooking(id);
        //    return RedirectToAction(controllerName: "Booking", actionName: "GetAllBookings");
        //}
        public async Task<IActionResult> Delete(int id)
        {
            // Check if the booking exists
            var booking = await this._repo.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }

            // Delete the booking from the database
            await this._repo.DeleteBooking(id);

            // Redirect to the list of bookings
            return RedirectToAction("GetAllBookings");
        }

        [HttpGet]
        public IActionResult Create()
        {
            //List<ApplicationUser> userlist = new List<ApplicationUser>();
            //userlist = _userManager.Users.ToList();
            //ViewBag.listofUser = userlist;

            //List<Room> li = new List<Room>();
            //li = _context.Room.ToList();
            //ViewBag.listofroom = li;
            return View();
        }

        [HttpGet]
        public IActionResult CreateRoomBooking(int roomId)
        {
            ViewData["RoomId"] = roomId;
            return View();
        }


        [HttpPost]
        public IActionResult CreateRoomBooking(int roomId, Booking booking)
        {
           

            if (ModelState.IsValid)
            {
                var userId = _UserService.GetUserId();
                booking.UserId = userId;
                booking.RoomId = roomId;
                // Save the booking to the database
                _context.Booking.Add(booking);
                _context.SaveChanges();

                var room = _context.Room.FirstOrDefault(r => r.Id == roomId);
                if (room != null)
                {
                    room.Status = false;
                    _context.Room.Update(room);
                    _context.SaveChanges();
                }

                TempData["BookingMessage"] = "Booking successfully created.";

                // Redirect to a thank-you page or back to the room list page
                return RedirectToAction("GetAllBookings");
            }

            ViewData["Message"] = "Data is not valid to create the booking";
            return View();
        }


        [HttpPost]
        public IActionResult Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                var book = _repo.AddBooking(booking);
                //var room = _Roomrepo.GetRoomById(booking.RoomId);

                //// Update the room status to false
                //var rooms = new Room { Status = room.Status.Equals(false) };
                //_Roomrepo.UpdateRoom(rooms.Id, rooms);
                return RedirectToAction("GetAllBookings");
            }
            ViewData["Message"] = "Data is not valid to create the booking";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            List<Room> li = new List<Room>();
            li = _context.Room.ToList();
            ViewBag.listofroom = li;

            List<ApplicationUser> userlist = new List<ApplicationUser>();
            userlist = _userManager.Users.ToList();
            ViewBag.listofUser = userlist;

            var old = await this._repo.GetBookingById(id);
            return View(old);

        }


        [HttpPost]
        public async Task<IActionResult> Update(Booking booking)
        {
            
            await _repo.UpdateBooking(booking.Id,booking);
            return RedirectToAction("GetAllBookings");
        }
      
        
    }
}
