using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class OrderViewModel
    {

        public OrderViewModel(Order obj)
        {
            Refresh(obj);
        }

        public OrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Person Entleiher { get; set; }

        public Person Verwalter { get; set; }

        public Device Device { get; set; }
        public Guid OrderGuid { get; set; }


        public void Refresh(Order obj)
        {

            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.OrderGuid = source.OrderGuid;
            target.Status = source.Status;
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.Entleiher = source.Entleiher;
            target.Verwalter = source.Verwalter;
            target.Device = source.Device;

        }

        public void ApplyChanges(Order obj)
        {
            var target = obj;
            var source = this;

            target.Status = source.Status;
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.Entleiher = source.Entleiher;
            target.Verwalter = source.Verwalter;
            target.Device = source.Device;

        }
    }
}