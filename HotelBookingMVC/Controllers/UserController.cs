using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using HotelBookingMVC.Models;
using HotelBookingMVC.CustomModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.Ajax.Utilities;
using HotelBookingMVC.Models.Enums;

namespace HotelBookingMVC.Controllers
{
    [CustomAuthorize(Roles = "User")]
    public class UserController : Controller
    {
        HotelConnection _context = new HotelConnection();
        public ActionResult ManageOrders()
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            List<GetUserOrders_Result> ResOrders;
            List<OrderToDisplay> Orders = new List<OrderToDisplay>();
            List<OrderToDisplay> BookingsCancelled = new List<OrderToDisplay>();
            List<OrderToDisplay> BookingsValid = new List<OrderToDisplay>();
            List<OrderToDisplay> PastOrders;
            List<OrderToDisplay> FutureOrders;


            ResOrders = _context.GetUserOrders(User.Identity.GetUserId()).ToList();
            foreach (GetUserOrders_Result r in ResOrders)
            {
                var CurRoom = _context.GetRoomById(r.RoomId).FirstOrDefault();

                OrderToDisplay OrderVM = new OrderToDisplay
                {
                    ID = r.ID,
                    Username = User.Identity.GetUserName(),
                    RoomNo = CurRoom.RoomNo,
                    FromDate = r.FromDate,
                    ToDate = r.ToDate,
                    IsCancelled = r.IsCancelled ? "Cancelled" : "Normal",
                    UnitPrice = CurRoom.Price,
                    TotalCost = r.TotalCost
                };
                if (r.IsCancelled)
                {
                    BookingsCancelled.Add(OrderVM);
                }
                else
                {
                    BookingsValid.Add(OrderVM);
                }
            }


            DateTime now = DateTime.Now;
            DateTime DateOfToday = new DateTime(now.Year, now.Month, now.Day);
            PastOrders = BookingsValid.Where(o => ((o.ToDate <= DateOfToday) || (o.FromDate <= DateOfToday && o.ToDate >= DateOfToday))).ToList();
            HashSet<int> PastCurBookingIds = new HashSet<int>(PastOrders.Select(o => o.ID).ToList());
            FutureOrders = BookingsValid.Where(o => !PastCurBookingIds.Contains(o.ID)).ToList();

            var tables = new ManageOrderViewModel
            {
                PastOrders = PastOrders,
                FutureOrders = FutureOrders,
                CancelledOrders = BookingsCancelled
            };

            return View(tables);
        }

        [HttpPost]
        public ActionResult CancelAnOrder(int OrderID)
        {
            var MatchedOrder = _context.Orders.FirstOrDefault(o => o.ID == OrderID);
            if (MatchedOrder != null)
            {
                _context.CancelAnOrder(OrderID);
            }
            else
            {
                ModelState.AddModelError("", "Order " + OrderID + " does not exist in database!"); ;
            }
            return RedirectToAction("ManageOrders");
        }

        public ActionResult BookRoom()
        {
            BookRoomViewModel vm = new BookRoomViewModel();
            vm.FromDate = DateTime.Now;
            vm.ToDate = DateTime.Now.AddDays(1);
            return View(vm);
        }

        [HttpPost]
        public ActionResult GetQualifiedRooms(BookRoomViewModel model)
        {
            List<FetchedRoomViewModel> Rooms = new List<FetchedRoomViewModel>();
            if (ModelState.IsValid)
            {
                int CurMinPrice = model.MinPrice == null ? 0 : (int)model.MinPrice;
                int CurMaxPrice = model.MaxPrice == null ? int.MaxValue : (int)model.MaxPrice;

                if (model.MinPrice != null && model.MaxPrice != null && model.MinPrice > model.MaxPrice)
                {
                    CurMaxPrice = (int)model.MinPrice;
                    CurMinPrice = (int)model.MaxPrice;
                    model.MinPrice = CurMaxPrice;
                    model.MaxPrice = CurMinPrice;
                }
                IEnumerable<GetQualifiedRooms_Result> ResRooms = _context.GetQualifiedRooms(model.FromDate, model.ToDate, model.IsACEquipped, CurMinPrice, CurMaxPrice, model.RmType, model.NoOfBeds);
                if (model.IsACEquipped != null)
                {
                    ResRooms = ResRooms.Where(r => r.IsACEquipped == model.IsACEquipped);
                }
                if (model.RmType != null) 
                {
                    ResRooms = ResRooms.Where(r => r.RmType == model.RmType);
                }
                if (model.NoOfBeds != null)
                {
                    ResRooms = ResRooms.Where(r => r.NoOfBeds == model.NoOfBeds);
                }
                foreach (var r in ResRooms) 
                {
                    FetchedRoomViewModel RoomVM = new FetchedRoomViewModel
                    {
                        ID = r.ID,
                        RoomNo = r.RoomNo,
                        IsACEquipped = r.IsACEquipped ? "Yes" : "No",
                        Price = r.Price,
                        RmType = Enum.GetName(typeof(RoomType), r.RmType),
                        NoOfBeds = Enum.GetName(typeof(BedNo), r.NoOfBeds),
                        TotalCost = r.Price * ((int)((model.ToDate - model.FromDate).TotalDays))
                    };
                    Rooms.Add(RoomVM);
                }
            }

            model.Rooms = Rooms;

            return View("BookRoom", model);
        }

        [HttpPost]
        public ActionResult ConfirmInfo(int RoomId, DateTime from, DateTime to, int total)
        {
            var res = _context.ReserveRoom(User.Identity.GetUserId(), RoomId, from, to, false, total);

            return RedirectToAction("ManageOrders");
        }
    }
}