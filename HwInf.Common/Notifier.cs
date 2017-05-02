using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common
{
    public class Notifier
    {
        private readonly HwInfContext db = new HwInfContext();
        private readonly BL.BL bl;
        public Notifier(DateTime date, string daysbefore)
        {
            bl = new BL.BL(db);

            DateTime reminddate = getReminderDate(date, daysbefore);
            var orderlist = bl.GetOrders().Where(i => i.To.ToShortDateString() == reminddate.ToShortDateString()).Select(i => i.OrderGuid).ToList();
            if (orderlist.Count() > 0)
            {
                foreach (var order in orderlist)
                {
                    Mail mail = new Mail(order);
                    mail.ReminderMessage(order);
                    mail.Send();
                }
            }
        }

        public DateTime getReminderDate(DateTime date, string daysbefore)
        {
            return date.AddDays(Int32.Parse(daysbefore));
        }
    }
}
