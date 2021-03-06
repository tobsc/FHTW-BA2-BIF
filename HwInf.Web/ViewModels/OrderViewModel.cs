﻿using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using MoreLinq;

namespace HwInf.Web.ViewModels
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

        public UserViewModel Entleiher { get; set; }
        public UserViewModel Verwalter { get; set; }
        public ICollection<OrderItemViewModel> OrderItems { get; set; }
        public Guid OrderGuid { get; set; }
        public string OrderReason { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime ReturnDate { get; set; }


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
            target.OrderItems = source.OrderItems.Select(i => new OrderItemViewModel(i)).ToList();
            target.Verwalter = new UserViewModel(source.Verwalter);
            target.Entleiher = new UserViewModel(source.Entleiher);
            target.OrderReason = source.OrderReason;
            target.OrderStatus = source.OrderStatus;
            target.ReturnDate = source.ReturnDate;

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
            target.Entleiher = source.Entleiher;
            target.Verwalter = source.Verwalter;
            target.OrderReason = source.OrderReason;
            target.OrderStatus = source.OrderStatus;
            target.ReturnDate = source.ReturnDate;
        }

        public void ApplyChanges(Order obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Date = DateTime.Now;
            target.From = source.From;
            target.To = source.To;
            target.OrderGuid = Guid.NewGuid();
            if (bl.IsAdmin || bl.IsVerwalter)
            {
                target.Entleiher = bl.GetUsers(source.Entleiher.Uid);
            }
            else
            {
                target.Entleiher = bl.GetUsers(bl.GetCurrentUid());
            }
            CreateOrderItems(obj, bl);
            target.Verwalter = target.OrderItems.Select(i => i.Device.Person).FirstOrDefault();
            target.OrderReason = source.OrderReason;
            target.OrderStatus = source.OrderStatus == null
                ? bl.GetOrderStatus("offen")
                : bl.GetOrderStatus(source.OrderStatus.Slug);
            target.ReturnDate = source.ReturnDate;

        }

        public OrderViewModel LoadOrderItems(Order obj)
        {
            OrderItems = obj.OrderItems
                .Select(i => new OrderItemViewModel(i))
                .ToList();

            return this;
        }

        public void CreateOrderItems(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderItems = OrderItems.Select(i =>
                {
                    var newOrderItem = new OrderItem
                    {
                        Device = bl.GetSingleDevice(i.Device.DeviceId),
                        From = obj.From,
                        To = obj.To,
                        Entleiher = bl.GetUsers(obj.Entleiher.Uid),
                        Verwalter = bl.GetUsers(i.Device.Verwalter.Uid),
                        CreateDate = DateTime.Now,
                        Accessories = i.Accessories.Any() ? string.Join(",", i.Accessories) : ""
                    };
                    newOrderItem.Device.Quantity = i.Device.Quantity;
                    return newOrderItem;
                })
            .ToList();

            var ordersWithoutInvNum = obj.OrderItems
                .Where(x => string.IsNullOrWhiteSpace(x.Device.InvNum))
                .DistinctBy(x => x.Device.DeviceGroupSlug)
                .ToList();

            ordersWithoutInvNum.ForEach(orderItem =>
            {
                var devices = bl.GetDevices().ToList();
                var test = devices
                    .Where(d => d.DeviceGroupSlug == orderItem.Device.DeviceGroupSlug)
                    .Take(orderItem.Device.Quantity)
                    .ToList();
                test.ForEach(d =>
                {
                    var newOrderItem = new OrderItem
                    {
                        Device = d,
                        From = obj.From,
                        To = obj.To,
                        Entleiher = bl.GetUsers(obj.Entleiher.Uid),
                        Verwalter = bl.GetUsers(orderItem.Device.Person.Uid),
                        CreateDate = DateTime.Now,
                        Accessories = orderItem.Accessories.Any() ? string.Join(",", orderItem.Accessories) : ""
                    };
                    obj.OrderItems.Add(newOrderItem);
                });

            });
            obj.OrderItems = obj.OrderItems.DistinctBy(i => i.Device.DeviceId).ToList();
        }

        public void Return(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("abgeschlossen");
            obj.OrderItems
                .Where(i => !i.IsDeclined)
                .ToList()
                .ForEach(i => i.Device.Status = bl.GetDeviceStatus(1));
            obj.ReturnDate = DateTime.Now;
        }

        public void Accept(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("akzeptiert");
            obj.OrderItems
                .Where(i => !i.IsDeclined)
                .ToList()
                .ForEach(i => i.Device.Status = bl.GetDeviceStatus(2));
        }

        public void Abort(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("abgebrochen");
            
            obj.ReturnDate = DateTime.Now;
        }

        public void Lend(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("ausgeliehen");
        }
        public void Decline(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("abgelehnt");
            obj.ReturnDate = DateTime.Now;
        }

        public void Reset(Order obj, IBusinessLogicFacade bl)
        {
            obj.OrderStatus = bl.GetOrderStatus("offen");
            obj.OrderItems
                .Where(i => !i.IsDeclined)
                .ToList()
                .ForEach(i =>
                {
                    i.Device.Status = bl.GetDeviceStatus(1);
                    i.IsDeclined = false;
                });
        }
    }
}