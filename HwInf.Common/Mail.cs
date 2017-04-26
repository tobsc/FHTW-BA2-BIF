using HwInf.Common.DAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HwInf.Common.BL;
using HwInf.Common.Models;

namespace HwInf.Common
{
    public class Mail
    {
        private SmtpClient smtpClient;
        private MailMessage mail;
        private readonly IDAL _db;
        private readonly BL.BL _bl;
        private readonly ILog _log = LogManager.GetLogger(typeof(Mail).Name);

        public Mail(int orderId)
        {
            _db = new HwInfContext();
            _bl = new BL.BL(_db);
            smtpClient = new SmtpClient("localhost", 25);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;

            Order order = _bl.GetOrders(orderId);
            string to = order.Entleiher.Email;
            string from = "hwinf@technikum-wien.at";


            mail = new MailMessage(from, to);
            mail.Subject = "HwInf";
            mail.SubjectEncoding = Encoding.UTF8;
            
           
            mail.BodyEncoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        }


        public void To(string to)
        {
            mail.To.Add(new MailAddress(to));
        }

       public void MessageFormat(string status, int orderId)
        {
            Order order = _bl.GetOrders(orderId);
            switch (status)
            {
                case "accept": AcceptMessage(order); break;
                case "decline": DeclineMessage(order); break;
                case "newOrder": NewOrderMessage(order); break;
                default: mail.Body = status; break;
            }
        }

        public void AcceptMessage(Order order)
        {
            mail.Body = "Ihre Anfrage für die angesuchten Geräte wurde bearbeitet: <br />";
            foreach (OrderItem ord in order.OrderItems)
            {
                //  if(!ord.Device.IsDeclined)
                mail.Body = ord.Device.Name + " : akzeptiert <br />";
                //else
                // mail.Body = ord.Device.Name + " : abgelehnt <br />";
            }
        }

        public void DeclineMessage(Order order)
        {
            mail.Body = "Ihre Anfrage wurde abgelehnt. Für Details besuchen Sie bitte <LinkZurSeite>";
        }
        public void NewOrderMessage(Order order)
        {
            mail.Body = "Es wurde eine neue Anfrage für eines/mehrere Ihrer Geräte erstellt";
            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body = "Name : "+ord.Device.Name +" | InventarNummer: "+ ord.Device.InvNum+ "<br />";
            }
        }


        public void Send()
        {
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception  ex)
            {
                _log.InfoFormat("Fehler beim senden" + ex.ToString());
            }
            
        }
    }
}
