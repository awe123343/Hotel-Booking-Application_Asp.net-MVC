using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HotelBookingMVC.CustomModel;

namespace HotelBookingMVC.Models
{
    public class ManageOrderViewModel
    {
        public IEnumerable<OrderToDisplay> PastOrders { get; set; }
        public IEnumerable<OrderToDisplay> FutureOrders { get; set; }
        public IEnumerable<OrderToDisplay> CancelledOrders { get; set; }
    }

    public class BookRoomViewModel
    {
        [Required]
        [Display(Name = "From")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime FromDate { get; set; }

        [Required]
        [Display(Name = "To")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MMM dd, yyyy}")]
        public System.DateTime ToDate { get; set; }

        [Display(Name = "Room Type")]
        public int? RmType { get; set; }
        
        [Display(Name = "No. of Beds")]
        public int? NoOfBeds { get; set; }

        [Display(Name = "Minimum Price($)")]
        public int? MinPrice { get; set; }

        [Display(Name = "Maximum Price($)")]
        public int? MaxPrice { get; set; }

        [Display(Name = "AC Equipped?")]
        public bool? IsACEquipped { get; set; }

        public List<FetchedRoomViewModel> Rooms { get; set; }
    }

    public class FetchedRoomViewModel
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Room No.")]
        public int RoomNo { get; set; }

        [Display(Name = "Has AC?")]
        public string IsACEquipped { get; set; }

        [Display(Name = "Price per day")]
        [DataType(DataType.Currency)]
        public int Price { get; set; }

        [Display(Name = "Room Type")]
        public string RmType { get; set; }

        [Display(Name = "No. of Beds")]
        public string NoOfBeds { get; set; }

        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public int TotalCost { get; set; }
    }
}