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


        }

        public void ApplyChanges(OrderItem obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Device = bl.GetSingleDevice(source.Device.InvNum);
            target.OrderStatus = bl.GetOrderStatus(obj.OrderStatus.Slug);
        }
    }
}