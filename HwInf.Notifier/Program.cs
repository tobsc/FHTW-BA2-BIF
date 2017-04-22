using HwInf.Common;
using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("We now create the Notifier");
            Notifier notify = new Notifier(DateTime.Now.Date);
            
        }
    }

    public class Notifier
    {
        
        public Notifier(DateTime date)
        {
            /*HwInfContext db = new HwInfContext();
            Console.WriteLine("We now are looking into the DB");
            var uidlist = db.Orders.Where(i => i.ReturnDate == date).Select(i => i.Verwalter.Uid).ToList();
            Console.WriteLine("We now create the Mail");
            if (uidlist.Count() > 0)
            {
                foreach (var uid in uidlist)
                {
                    var mailadress = db.Persons.Where(i => i.Uid == uid).Select(i => i.Email).SingleOrDefault();
                    var act = @"Liebe/r StudentIn, 
                    Sie haben bei uns ein oder mehrere Geräte ausgeborgt, und heute ist der vereinbarte Termin für die Rückgabe.
                    Bitte vergessen Sie es nicht!
                    Mit freundlichen Grüßen,
                    Ihr HW-Inf Team
                
                    PS: Dies ist eine automatische Mail. Bitte nicht darauf Antowrten";

                    Mail mail = new Mail();
                    mail.To(mailadress);
                    mail.Message(act);
                    mail.Send();

                }
            }
            else
            {
                Console.WriteLine("No Orders expiring today");
            }
            */
        }
    }
}
