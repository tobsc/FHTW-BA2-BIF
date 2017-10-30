using HwInf.Common;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using HwInf.Common.Interfaces;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataAccessLayer db = new HwInfContext();
            IBusinessLayer bl = new BusinessLayer(db);
            var notify = new Notifier(bl);
            notify.Send();
            var reminder = new Reminder(bl);
            reminder.SendReminder();
            Console.ReadKey();       
        }
    }

    public class Notifier
    {
        private readonly List<Guid> _orderList;
        private readonly IBusinessLayer _bl;
        public Notifier(IBusinessLayer bl)
        {
            _bl = bl;
            var remindDate = GetReminderDate(bl.GetSetting("days_before_reminder").Value);
            _orderList = bl.GetOrders()
                .Where(i => i.To.Date == remindDate.Date)
                .Where(i => i.OrderStatus.Slug.Equals("ausgeliehen"))
                .Select(i => i.OrderGuid)
                .ToList();
        }

        public DateTime GetReminderDate(string daysbefore)
        {     
            return DateTime.Now.Date.AddDays(Int32.Parse(daysbefore));
        }
        public void Send()
        {
            if (!_orderList.Any()) return;
            foreach (var order in _orderList)
            {
                Mail mail = new Mail(order, _bl);
                mail.ReminderMessage(order);
                mail.Send();
            }
        }
    }

    public class Reminder
    {
        private readonly List<Guid> _orderList;
        private readonly IBusinessLayer _bl;
        public Reminder(IBusinessLayer bl)
        {
            _bl = bl;
            _orderList = bl.GetOrders()
                .Where(i => i.To.Date.AddDays(1) == DateTime.Now.Date)
                .Where(i => i.OrderStatus.Slug.Equals("ausgeliehen"))
                .Select(i => i.OrderGuid)
                .ToList();
        }

        public void SendReminder()
        {
            if (!_orderList.Any()) return;
            foreach (var order in _orderList)
            {
                Mail mail = new Mail(order, _bl);
                mail.ReminderMessage(order);
                mail.Send();
            }
        }
    }
}
