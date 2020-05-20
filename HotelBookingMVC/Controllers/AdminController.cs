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
    [CustomAuthorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        HotelConnection _context = new HotelConnection();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult ManageRooms()
        {
            List<GetRooms_Result> ResRooms;
            ResRooms = _context.GetRooms().ToList();
            List<ManageRoomViewModel> Rooms = new List<ManageRoomViewModel>();

            foreach (GetRooms_Result r in ResRooms)
            {
                ManageRoomViewModel RoomVM = new ManageRoomViewModel
                {
                    ID = r.ID,
                    RoomNo = r.RoomNo,
                    IsACEquipped = r.IsACEquipped ? "Yes" : "No",
                    IsDisabled = r.IsDisabled ? "Disabled" : "Normal",
                    Price = r.Price,
                    RmType = Enum.GetName(typeof(RoomType), r.RmType),
                    NoOfBeds = Enum.GetName(typeof(BedNo), r.NoOfBeds)
                };
                Rooms.Add(RoomVM);
            }
            return View(Rooms);
        }

        public ActionResult EditRoom(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            GetRoomById_Result RoomToEdit;
            RoomToEdit = _context.GetRoomById(id).FirstOrDefault();
            if (RoomToEdit == null)
            {
                return View("Error");
            }

            EditRoomViewModel RoomVM = new EditRoomViewModel
            {
                ID = RoomToEdit.ID,
                RoomNo = RoomToEdit.RoomNo,
                IsACEquipped = RoomToEdit.IsACEquipped,
                IsDisabled = RoomToEdit.IsDisabled,
                Price = RoomToEdit.Price,
                RmType = Enum.GetName(typeof(RoomType), RoomToEdit.RmType),
                NoOfBeds = Enum.GetName(typeof(BedNo), RoomToEdit.NoOfBeds),
            };
            return View(RoomVM);
        }

        [HttpPost]
        public ActionResult EditRoom(EditRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                int OriNo = _context.GetRoomById(model.ID).FirstOrDefault().RoomNo;
                HashSet<int> RoomNos = new HashSet<int>(_context.GetRooms().Select(r => r.RoomNo).Where(no => no != OriNo).ToList());
                if (RoomNos.Contains(model.RoomNo))
                {
                    ModelState.AddModelError("", "Room number exists!");
                    return View(model);
                }
                else
                {
                    _context.UpdateExistingRoom(model.ID, model.RoomNo, model.IsACEquipped, model.IsDisabled, model.Price, (int)Enum.Parse(typeof(RoomType), model.RmType), (int)Enum.Parse(typeof(BedNo), model.NoOfBeds));
                }

                return RedirectToAction("ManageRooms");
            }
            return View(model);
        }

        public ActionResult AddNewRoom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewRoom(EditRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                HashSet<int> RoomNos = new HashSet<int>(_context.GetRooms().Select(r => r.RoomNo).ToList());
                if (RoomNos.Contains(model.RoomNo))
                {
                    ModelState.AddModelError("", "Room number exists!");
                    return View(model);
                }
                else
                {
                    _context.AddNewRoom(model.RoomNo, model.IsACEquipped, model.IsDisabled, model.Price, (int)Enum.Parse(typeof(RoomType), model.RmType), (int)Enum.Parse(typeof(BedNo), model.NoOfBeds));
                }

                return RedirectToAction("ManageRooms");
            }
            return View(model);
        }

        public ActionResult ManageOrders()
        {
            List<GetOrders_Result> ResOrders;
            List<OrderToDisplay> Orders = new List<OrderToDisplay>();
            List<OrderToDisplay> CancelledOrders = new List<OrderToDisplay>();

            ResOrders = _context.GetOrders().ToList();
            foreach (GetOrders_Result r in ResOrders)
            {
                var CurRoom = _context.GetRoomById(r.RoomId).FirstOrDefault();

                OrderToDisplay OrderVM = new OrderToDisplay
                {
                    ID = r.ID,
                    Username = UserMgr.FindById(r.UserId).UserName,
                    RoomNo = CurRoom.RoomNo,
                    FromDate = r.FromDate,
                    ToDate = r.ToDate,
                    IsCancelled = r.IsCancelled ? "Cancelled" : "Normal",
                    UnitPrice = CurRoom.Price,
                    TotalCost = r.TotalCost
                };
                if (!r.IsCancelled)
                {
                    Orders.Add(OrderVM);
                }
                else
                {
                    CancelledOrders.Add(OrderVM);
                }
            }

            var tables = new ManageOrdersViewModel
            {
                Orders = Orders,
                CancelledOrders = CancelledOrders
            };

            return View(tables);
        }

        public ActionResult EditOrder(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            GetOrderById_Result OrderToEdit;
            EditOrderViewModel OrderVM;

            OrderToEdit = _context.GetOrderById(id).FirstOrDefault();
            var CurRoom = _context.GetRoomById(OrderToEdit.RoomId).FirstOrDefault();

            if (OrderToEdit == null)
            {
                return View("Error");
            }
            OrderVM = new EditOrderViewModel
            {
                ID = OrderToEdit.ID,
                UserId = OrderToEdit.UserId,
                RoomId = OrderToEdit.RoomId,
                FromDate = OrderToEdit.FromDate,
                ToDate = OrderToEdit.ToDate,
                UnitPrice = CurRoom.Price,
                TotalCost = OrderToEdit.TotalCost,
                IsCancelled = OrderToEdit.IsCancelled
            };

            PopulateUserDropDownList(OrderVM.UserId);
            PopulateRoomDropDownList(OrderVM.RoomId);
            return View(OrderVM);
        }

        [HttpPost]
        public ActionResult EditOrder(EditOrderViewModel model)
        {
            GetOrderById_Result CurOrder = new HotelConnection().GetOrderById(model.ID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                List<int> UnAvailableRooms = _context.GetOrders().Where(order => ((order.ID != model.ID) && !order.IsCancelled && !(order.FromDate >= model.ToDate || order.ToDate <= model.FromDate))).Select(r => r.RoomId).ToList();
                HashSet<int> UnAvailableRoomIds = new HashSet<int>(UnAvailableRooms);

                if (UnAvailableRoomIds.Contains(model.RoomId))
                {
                    ModelState.AddModelError("", "Room not available at designated date range!");
                    return View(model);
                }
                else
                {
                    _context.UpdateExistingOrder(model.ID, model.UserId, model.RoomId, model.FromDate, model.ToDate, model.IsCancelled, model.TotalCost);
                }

                return RedirectToAction("ManageOrders");
            }
            PopulateUserDropDownList(CurOrder.UserId);
            PopulateRoomDropDownList(CurOrder.RoomId);
            return View(model);
        }

        private void PopulateUserDropDownList(object selectedUser = null)
        {
            var Users = UserMgr.Users.ToList().OrderBy(u => u.UserName);
            ViewBag.UserId = new SelectList(Users, "Id", "UserName", selectedUser);
        }

        private void PopulateRoomDropDownList(object selectedRoom = null)
        {
            List<GetRooms_Result> Rooms;


            Rooms = _context.GetRooms().OrderBy(r => r.RoomNo).ToList();

            ViewBag.RoomId = new SelectList(Rooms, "Id", "RoomNo", selectedRoom);
        }

        UserManager<IdentityUser> UserMgr = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
        RoleManager<IdentityRole> RoleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());

        public ActionResult ManageUsers()
        {
            var Users = UserMgr.Users.ToList();
            var Roles = RoleMgr.Roles.ToList();

            List<ExtendedUser> ResUsers = new List<ExtendedUser>();
            foreach (IdentityUser u in Users)
            {
                string RId = u.Roles.FirstOrDefault().RoleId;
                string RoleName = Roles.FirstOrDefault(r => r.Id == RId).Name;
                ExtendedUser ExtUser = new ExtendedUser
                {
                    ID = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Role = RoleName
                };
                ResUsers.Add(ExtUser);
            }

            ResUsers = ResUsers.OrderBy(u => u.UserName).ToList();

            return View(ResUsers);
        }

        public ActionResult EditUser(string id)
        {
            var UserToEdit = UserMgr.FindById(id);

            if (id == null || UserToEdit == null)
            {
                return View("Error");
            }

            var UserRole = RoleMgr.Roles.ToList().FirstOrDefault(r => r.Id == UserToEdit.Roles.FirstOrDefault().RoleId).Name;
            var UserRes = new EditUserViewModel
            {
                ID = UserToEdit.Id,
                UserName = UserToEdit.UserName,
                Email = UserToEdit.Email,
                Phone = UserToEdit.PhoneNumber,
                Password = "",
                Role = UserRole
            };

            return View(UserRes);
        }

        [HttpPost]
        public ActionResult EditUser(EditUserViewModel model)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            manager.UserValidator = new UserValidator<ApplicationUser>(manager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };

            if (ModelState.IsValid)
            {
                var user = manager.FindById(model.ID);

                Regex phoneRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                string FormarttedPhone = phoneRegex.Replace(model.Phone, "($1) $2-$3");

                user.PhoneNumber = FormarttedPhone;
                user.UserName = model.UserName;
                user.Email = model.Email;

                if (model.Password != null && model.Password.Length > 0)
                {
                    user.PasswordHash = manager.PasswordHasher.HashPassword(model.Password);
                }

                var RoleId = user.Roles.SingleOrDefault().RoleId;
                var RoleName = RoleMgr.Roles.SingleOrDefault(r => r.Id == RoleId).Name;
                if (RoleName != model.Role)
                {
                    UserMgr.RemoveFromRole(model.ID, RoleName);
                    UserMgr.AddToRole(model.ID, model.Role);
                }

                IdentityResult UpdateRes = manager.Update(user);
                if (!UpdateRes.Succeeded)
                {
                    foreach (var error in UpdateRes.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(model);
                }
                if (RoleName != model.Role && user.Id == User.Identity.GetUserId()) 
                {
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return RedirectToAction("Index", "Home");
                }

                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

        public ActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNewUser(AddNewUserViewModel model)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            manager.UserValidator = new UserValidator<ApplicationUser>(manager) { AllowOnlyAlphanumericUserNames = false, RequireUniqueEmail = true };

            if (ModelState.IsValid)
            {
                Regex phoneRegex = new Regex(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
                string FormarttedPhone = phoneRegex.Replace(model.Phone, "($1) $2-$3");

                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, PhoneNumber = FormarttedPhone };
                IdentityResult result = manager.Create(user, model.Password);
                if (result.Succeeded)
                {
                    manager.AddToRole(user.Id, model.Role);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View(model);
                }

                return RedirectToAction("ManageUsers");
            }
            return View(model);
        }

    }
}