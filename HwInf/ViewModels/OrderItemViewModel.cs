using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;

namespace HwInf.ViewModels
{
    public class OrderItemViewModel
    {

        public int ItemId { get; set; }
        public OrderStatusViewModel OrderStatus { get; set; }
        public DeviceViewModel Device { get; set; } 
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime CreateDate { get; set; }

        public OrderItemViewModel(OrderItem obj)
        {
            Refresh(obj);
        }

        public void Refresh(OrderItem obj)
        {

            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.OrderStatus = new OrderStatusViewModel(source.OrderStatus);
            target.Device = new DeviceViewModel(source.Device);
            target.ItemId = source.ItemId;
            target.To = source.To;
            target.From = source.From;
            target.CreateDate = source.CreateDate;

        }

        public void ApplyChanges(OrderItem obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Device = bl.GetSingleDevice(source.Device.InvNum);
            
            target.OrderStatus = target.OrderStatus == null
                ? bl.GetOrderStatus("offen")
                : bl.GetOrderStatus(target.OrderStatus.Slug);

            if (target.CreateDate == DateTime.MinValue)
            {
                target.CreateDate = DateTime.Now;
            } 
        }
    }
}