﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HotelBookingMVC.CustomModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HotelConnection : DbContext
    {
        public HotelConnection()
            : base("name=HotelConnection")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
    
        public virtual int AddNewRoom(Nullable<int> newRoomNo, Nullable<bool> newIsACEquipped, Nullable<bool> newIsDisabled, Nullable<int> newPrice, Nullable<int> newRmType, Nullable<int> newNoOfBeds)
        {
            var newRoomNoParameter = newRoomNo.HasValue ?
                new ObjectParameter("NewRoomNo", newRoomNo) :
                new ObjectParameter("NewRoomNo", typeof(int));
    
            var newIsACEquippedParameter = newIsACEquipped.HasValue ?
                new ObjectParameter("NewIsACEquipped", newIsACEquipped) :
                new ObjectParameter("NewIsACEquipped", typeof(bool));
    
            var newIsDisabledParameter = newIsDisabled.HasValue ?
                new ObjectParameter("NewIsDisabled", newIsDisabled) :
                new ObjectParameter("NewIsDisabled", typeof(bool));
    
            var newPriceParameter = newPrice.HasValue ?
                new ObjectParameter("NewPrice", newPrice) :
                new ObjectParameter("NewPrice", typeof(int));
    
            var newRmTypeParameter = newRmType.HasValue ?
                new ObjectParameter("NewRmType", newRmType) :
                new ObjectParameter("NewRmType", typeof(int));
    
            var newNoOfBedsParameter = newNoOfBeds.HasValue ?
                new ObjectParameter("NewNoOfBeds", newNoOfBeds) :
                new ObjectParameter("NewNoOfBeds", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("AddNewRoom", newRoomNoParameter, newIsACEquippedParameter, newIsDisabledParameter, newPriceParameter, newRmTypeParameter, newNoOfBedsParameter);
        }
    
        public virtual int CancelAnOrder(Nullable<int> orderId)
        {
            var orderIdParameter = orderId.HasValue ?
                new ObjectParameter("OrderId", orderId) :
                new ObjectParameter("OrderId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CancelAnOrder", orderIdParameter);
        }
    
        public virtual ObjectResult<GetOrders_Result> GetOrders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetOrders_Result>("GetOrders");
        }
    
        public virtual ObjectResult<GetQualifiedRooms_Result> GetQualifiedRooms(Nullable<System.DateTime> fromTime, Nullable<System.DateTime> toTime, Nullable<bool> hasAC, Nullable<int> minPrice, Nullable<int> maxPrice, Nullable<int> rType, Nullable<int> bNo)
        {
            var fromTimeParameter = fromTime.HasValue ?
                new ObjectParameter("FromTime", fromTime) :
                new ObjectParameter("FromTime", typeof(System.DateTime));
    
            var toTimeParameter = toTime.HasValue ?
                new ObjectParameter("ToTime", toTime) :
                new ObjectParameter("ToTime", typeof(System.DateTime));
    
            var hasACParameter = hasAC.HasValue ?
                new ObjectParameter("HasAC", hasAC) :
                new ObjectParameter("HasAC", typeof(bool));
    
            var minPriceParameter = minPrice.HasValue ?
                new ObjectParameter("MinPrice", minPrice) :
                new ObjectParameter("MinPrice", typeof(int));
    
            var maxPriceParameter = maxPrice.HasValue ?
                new ObjectParameter("MaxPrice", maxPrice) :
                new ObjectParameter("MaxPrice", typeof(int));
    
            var rTypeParameter = rType.HasValue ?
                new ObjectParameter("RType", rType) :
                new ObjectParameter("RType", typeof(int));
    
            var bNoParameter = bNo.HasValue ?
                new ObjectParameter("BNo", bNo) :
                new ObjectParameter("BNo", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetQualifiedRooms_Result>("GetQualifiedRooms", fromTimeParameter, toTimeParameter, hasACParameter, minPriceParameter, maxPriceParameter, rTypeParameter, bNoParameter);
        }
    
        public virtual ObjectResult<GetRoomById_Result> GetRoomById(Nullable<int> rId)
        {
            var rIdParameter = rId.HasValue ?
                new ObjectParameter("RId", rId) :
                new ObjectParameter("RId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetRoomById_Result>("GetRoomById", rIdParameter);
        }
    
        public virtual ObjectResult<GetRooms_Result> GetRooms()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetRooms_Result>("GetRooms");
        }
    
        public virtual ObjectResult<GetUserOrders_Result> GetUserOrders(string userId)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUserOrders_Result>("GetUserOrders", userIdParameter);
        }
    
        public virtual int ReserveRoom(string userId, Nullable<int> roomId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<bool> isCancelled, Nullable<int> totalCost)
        {
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            var roomIdParameter = roomId.HasValue ?
                new ObjectParameter("RoomId", roomId) :
                new ObjectParameter("RoomId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var isCancelledParameter = isCancelled.HasValue ?
                new ObjectParameter("IsCancelled", isCancelled) :
                new ObjectParameter("IsCancelled", typeof(bool));
    
            var totalCostParameter = totalCost.HasValue ?
                new ObjectParameter("TotalCost", totalCost) :
                new ObjectParameter("TotalCost", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ReserveRoom", userIdParameter, roomIdParameter, fromDateParameter, toDateParameter, isCancelledParameter, totalCostParameter);
        }
    
        public virtual int UpdateExistingOrder(Nullable<int> orderId, string userId, Nullable<int> roomId, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, Nullable<bool> isCancelled, Nullable<int> totalCost)
        {
            var orderIdParameter = orderId.HasValue ?
                new ObjectParameter("OrderId", orderId) :
                new ObjectParameter("OrderId", typeof(int));
    
            var userIdParameter = userId != null ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(string));
    
            var roomIdParameter = roomId.HasValue ?
                new ObjectParameter("RoomId", roomId) :
                new ObjectParameter("RoomId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("FromDate", fromDate) :
                new ObjectParameter("FromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("ToDate", toDate) :
                new ObjectParameter("ToDate", typeof(System.DateTime));
    
            var isCancelledParameter = isCancelled.HasValue ?
                new ObjectParameter("IsCancelled", isCancelled) :
                new ObjectParameter("IsCancelled", typeof(bool));
    
            var totalCostParameter = totalCost.HasValue ?
                new ObjectParameter("TotalCost", totalCost) :
                new ObjectParameter("TotalCost", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateExistingOrder", orderIdParameter, userIdParameter, roomIdParameter, fromDateParameter, toDateParameter, isCancelledParameter, totalCostParameter);
        }
    
        public virtual int UpdateExistingRoom(Nullable<int> roomId, Nullable<int> newRoomNo, Nullable<bool> newIsACEquipped, Nullable<bool> newIsDisabled, Nullable<int> newPrice, Nullable<int> newRmType, Nullable<int> newNoOfBeds)
        {
            var roomIdParameter = roomId.HasValue ?
                new ObjectParameter("RoomId", roomId) :
                new ObjectParameter("RoomId", typeof(int));
    
            var newRoomNoParameter = newRoomNo.HasValue ?
                new ObjectParameter("NewRoomNo", newRoomNo) :
                new ObjectParameter("NewRoomNo", typeof(int));
    
            var newIsACEquippedParameter = newIsACEquipped.HasValue ?
                new ObjectParameter("NewIsACEquipped", newIsACEquipped) :
                new ObjectParameter("NewIsACEquipped", typeof(bool));
    
            var newIsDisabledParameter = newIsDisabled.HasValue ?
                new ObjectParameter("NewIsDisabled", newIsDisabled) :
                new ObjectParameter("NewIsDisabled", typeof(bool));
    
            var newPriceParameter = newPrice.HasValue ?
                new ObjectParameter("NewPrice", newPrice) :
                new ObjectParameter("NewPrice", typeof(int));
    
            var newRmTypeParameter = newRmType.HasValue ?
                new ObjectParameter("NewRmType", newRmType) :
                new ObjectParameter("NewRmType", typeof(int));
    
            var newNoOfBedsParameter = newNoOfBeds.HasValue ?
                new ObjectParameter("NewNoOfBeds", newNoOfBeds) :
                new ObjectParameter("NewNoOfBeds", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateExistingRoom", roomIdParameter, newRoomNoParameter, newIsACEquippedParameter, newIsDisabledParameter, newPriceParameter, newRmTypeParameter, newNoOfBedsParameter);
        }
    
        public virtual ObjectResult<GetOrderById_Result> GetOrderById(Nullable<int> oId)
        {
            var oIdParameter = oId.HasValue ?
                new ObjectParameter("OId", oId) :
                new ObjectParameter("OId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetOrderById_Result>("GetOrderById", oIdParameter);
        }
    }
}