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

        public int OrderId { get; set; }

        public string Status { get; set; }
        public int StatusId { get; set; }

        public DateTime Date { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Person { get; set; }

        public string PersonUid { get; set; }

        public string Owner { get; set; }

        public string OwnerUid { get; set; }

        public List<int> OrderItems { get; set; }



        public void Refresh(Order obj)
        {

            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.OrderId = source.OrderId;
            target.Status = source.Status.Description;
            target.StatusId = source.Status.StatusId;
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.Person = source.Person.Name;
            target.PersonUid = source.Person.uid;
            target.Owner = source.Owner.Name;
            target.OwnerUid = source.Owner.uid;

        }

        public void ApplyChanges(Order obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Status = db.OrderStatus.Single(i => i.Description == source.Status);
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.Person = db.Persons.Single(i => i.uid == source.PersonUid);
            target.Owner = db.Persons.Single(i => i.uid == source.OwnerUid);

        }

        public OrderViewModel LoadOrderItems(HwInfContext db)
        {
            var oItems = db.OrderItems.Include("Order").Where(i => i.Order.OrderId == OrderId).Select(i => i.Device);
            OrderItems = new List<int>();

            if(oItems.Any())
            {
                foreach (var o in oItems)
                {
                    OrderItems.Add(o.DeviceId);
                }
            }


            return this;
        }

        public List<OrderItem> CreateOrderItems(Order o, HwInfContext db)
        {

            var oi = new List<OrderItem>();
            if (OrderItems != null)
            {
                oi.AddRange(OrderItems.Select(ois => new OrderItem
                {
                    Device = db.Devices.Single(i => i.DeviceId == ois), Order = db.Orders.Single(i => i.OrderId == o.OrderId)
                }));
            }

            return oi;
        }

        public void ChangeStatus(Order obj, HwInfContext db, string action)
        {
            var target = obj;
            var st = "";

            switch (action)
            {
                case "decline":
                    target.Status = db.OrderStatus.Single(i => i.Description == "Abgelehnt");
                    return;
                case "accept":
                    target.Status = db.OrderStatus.Single(i => i.Description == "Akzeptiert");
                    st = "Ausgeliehen";
                    break;
                case "return":
                    target.Status = db.OrderStatus.Single(i => i.Description == "Abgeschlossen");
                    st = "Verfügbar";
                    break;
            }

            var orderItems = LoadOrderItems(db);

            foreach (var id in orderItems.OrderItems)
            {

                var dev = db.Devices.Single(i => i.DeviceId == id);
                dev.Status = db.DeviceStatus.Single(i => i.Description == st);
            }
        }

        public void DeclineOrder(Order obj, HwInfContext db)
        {
            var target = obj;

            target.Status = db.OrderStatus.Single(i => i.Description == "Abgelehnt");
        }

        public bool ContainsDuplicates()
        {
            var groups = OrderItems.GroupBy(i => i);

            foreach(var i in groups)
            {
                if(i.Count() > 1)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ContainsLentItems(HwInfContext db)
        {
            return OrderItems.Any(devId => db.Devices.Where(i => i.DeviceId == devId).Select(i => i.Status.Description).SingleOrDefault() == "Ausgeliehen");
        }
    }
}