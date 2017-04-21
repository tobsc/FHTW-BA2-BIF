using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.BL;
using HwInf.Common.Models;
using WebGrease.Css.Extensions;

namespace HwInf.ViewModels
{
    public class OrderViewModel
    {

        public OrderViewModel()
        {
            
        }
        public OrderViewModel(Order obj)
        {
            Refresh(obj);
        }

        public OrderViewModel(OrderViewModel obj)
        {
            Refresh(obj);   
        }

        public int OrderId { get; set; }
        public DateTime Date { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string EntleiherUid { get; set; }

        public string VerwalterUid { get; set; }

        public ICollection<OrderItemViewModel> OrderItems { get; set; }
        public Guid OrderGuid { get; set; }
        public string OrderReason { get; set; }


        public void Refresh(Order obj)
        {

            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.OrderId = source.OrderId;
            target.OrderGuid = source.OrderGuid;
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.EntleiherUid = source.Entleiher.Uid;
            target.VerwalterUid = source.Verwalter.Uid;
            target.OrderReason = source.OrderReason;

        }

        public void Refresh(OrderViewModel obj)
        {
            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.OrderId = source.OrderId;
            target.OrderGuid = source.OrderGuid;
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.EntleiherUid = source.EntleiherUid;
            target.VerwalterUid = source.VerwalterUid;
            target.OrderReason = source.OrderReason;
        }

        public void ApplyChanges(Order obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Date = DateTime.Now;
            target.From = source.From;
            target.To = source.To;
            target.Entleiher = bl.GetUsers(bl.GetCurrentUid());
            target.OrderGuid = Guid.NewGuid();
            CreateOrderItems(obj, bl);
            target.Verwalter = target.OrderItems.Select(i => i.Device.Person).FirstOrDefault();
            target.OrderReason = source.OrderReason;


        }

        public OrderViewModel LoadOrderItems(Order obj)
        {
            OrderItems = obj.OrderItems
                .Select(i => new OrderItemViewModel(i))
                .ToList();

            return this;
        }

        public void CreateOrderItems(Order obj, BL bl)
        {
            obj.OrderItems = OrderItems.Select(i => new OrderItem
            {
                Device = bl.GetSingleDevice(i.Device.InvNum),
                OrderStatus = bl.GetOrderStatus("offen"),
                From = obj.From,
                To = obj.To
            })
            .ToList();
        }
    }
}