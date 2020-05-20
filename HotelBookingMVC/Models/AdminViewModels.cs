using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingMVC.Models
{
    public class ExtendedUser
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
        [Required]
        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }
    }

    public class EditUserViewModel
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter valid phone number. e.g. (XXX) YYY-ZZZZ.")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,16}", ErrorMessage = "Password must be 6-16 characters long. Include at least 1 number, 1 capital letter, 1 lower letter and 1 special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

    }

    public class AddNewUserViewModel
    {
        public string ID { get; set; }
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter valid phone number. e.g. (XXX) YYY-ZZZZ.")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{6,16}", ErrorMessage = "Password must be 6-16 characters long. Include at least 1 number, 1 capital letter, 1 lower letter and 1 special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

    }

    public class ManageRoomViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Room No.")]
        public int RoomNo { get; set; }
        [Required]
        [Display(Name = "Has AC?")]
        public string IsACEquipped { get; set; }
        [Required]
        [Display(Name = "Disabled?")]
        public string IsDisabled { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Unit Price")]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Room Type")]
        public string RmType { get; set; }
        [Required]
        [Display(Name = "No. of Beds")]
        public string NoOfBeds { get; set; }
    }

    public class EditRoomViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Room No.")]
        public int RoomNo { get; set; }
        [Required]
        [Display(Name = "Has AC?")]
        public Boolean IsACEquipped { get; set; }
        [Required]
        [Display(Name = "Disabled?")]
        public Boolean IsDisabled { get; set; }
        [Required]
        [Display(Name = "Unit Price")]
        public int Price { get; set; }
        [Required]
        [Display(Name = "Room Type")]
        public string RmType { get; set; }
        [Required]
        [Display(Name = "No. of Beds")]
        public string NoOfBeds { get; set; }
    }

    public class OrderToDisplay
    { 
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "User")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Room No.")]
        public int RoomNo { get; set; }
        [Required]
        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime FromDate { get; set; }
        [Required]
        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "Order status")]
        public string IsCancelled { get; set; }
        [Required]
        [Display(Name = "Price per day")]
        [DataType(DataType.Currency)]
        public int UnitPrice { get; set; }
        [Required]
        [Display(Name = "Final charge")]
        [DataType(DataType.Currency)]
        public int TotalCost { get; set; }
    }

    public class ManageOrdersViewModel
    { 
        public IEnumerable<OrderToDisplay> Orders { get; set; }
        public IEnumerable<OrderToDisplay> CancelledOrders { get; set; }

    }

    public class EditOrderViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
        [Display(Name = "Room No.")]
        public int RoomId { get; set; }
        [Required]
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime FromDate { get; set; }
        [Required]
        [Display(Name = "To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime ToDate { get; set; }
        [Required]
        [Display(Name = "Cancel order?")]
        public bool IsCancelled { get; set; }
        [Required]
        [Display(Name = "Unit price")]
        public int UnitPrice { get; set; }
        [Required]
        [Display(Name = "Total cost")]
        public int TotalCost { get; set; }
    }
}