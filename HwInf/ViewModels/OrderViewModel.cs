using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            target.Status = db.Status.Single(i => i.Description == source.Status);
            target.Date = source.Date;
            target.From = source.From;
            target.To = source.To;
            target.Person = db.Persons.Single(i => i.uid == source.PersonUid);
            target.Owner = db.Persons.Single(i => i.uid == source.OwnerUid);

        }

        public OrderViewModel loadOrderItems(HwInfContext db)
        {
            var oItems = db.OrderItems.Include("Order").Where(i => i.Order.OrderId == OrderId).Select(i => i.Device);
            OrderItems = new List<int>();

            if(oItems.Count() > 0)
            {
                foreach (var o in oItems)
                {
                    OrderItems.Add(o.DeviceId);
                }
            }


            return this;
        }

        public List<OrderItem> createOrderItems(Order o, HwInfContext db)
        {

            List<OrderItem> oi = new List<OrderItem>();
            if (OrderItems != null)
            {
                foreach (var ois in OrderItems)
                {
                    OrderItem tmp = new OrderItem();
                    tmp.Device = db.Devices.Single(i => i.DeviceId == ois);
                    tmp.Order = db.Orders.Single(i => i.OrderId == o.OrderId);

                    oi.Add(tmp);
                }
            }

            return oi;
        }

        public void changeStatus(Order obj, HwInfContext db, string action)
        {
            var target = obj;
            var source = this;
            var st = "";

            if (action == "decline")
            {
                target.Status = db.Status.Single(i => i.Description == "Abgelehnt");
                return;
            }


            if (action == "accept")
            {
                target.Status = db.Status.Single(i => i.Description == "Akzeptiert");
                st = "Ausgeliehen";
            }

            if(action == "return")
            {
                target.Status = db.Status.Single(i => i.Description == "Abgeschlossen");
                st = "Verfügbar";
            }

            var orderItems = loadOrderItems(db);

            foreach (var id in orderItems.OrderItems)
            {

                Device dev = db.Devices.Single(i => i.DeviceId == id);
                dev.Status = db.Status.Single(i => i.Description == st);
            }
        }

        public void declineOrder(Order obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Status = db.Status.Single(i => i.Description == "Abgelehnt");
        }
    }
}