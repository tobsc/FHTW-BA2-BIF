﻿using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
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
        public Person Entleiher { get; set; }
        public bool IsDeclined { get; set; } = false;
        public IEnumerable<string> Accessories { get; set; }

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

            target.Device = new DeviceViewModel(source.Device);
            target.ItemId = source.ItemId;
            target.To = source.To;
            target.From = source.From;
            target.CreateDate = source.CreateDate;
            target.Entleiher = source.Entleiher;
            target.ReturnDate = source.ReturnDate;
            target.IsDeclined = source.IsDeclined;
            target.Accessories = !String.IsNullOrWhiteSpace(source.Accessories) ? source.Accessories.Split(new char[] { ',' }).ToList(): null;
        }

        public void ApplyChanges(OrderItem obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Device = bl.GetSingleDevice(source.Device.DeviceId);
            target.IsDeclined = source.IsDeclined;
            target.Accessories = source.Accessories.Any() ? string.Join(",", source.Accessories) : "";

            if (target.CreateDate == DateTime.MinValue)
            {
                target.CreateDate = DateTime.Now;
            } 
        }
    }
}