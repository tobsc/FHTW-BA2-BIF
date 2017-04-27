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
        private Order _order;

        public Mail(Guid orderGuid)
        {

            _db = new HwInfContext();
            _bl = new BL.BL(_db);
            smtpClient = new SmtpClient("localhost", 8181);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;


            _order = _bl.GetOrders(orderGuid);
            string to = _order.Entleiher.Email;
            string from = "hwinf@technikum-wien.at";


            mail = new MailMessage(from, to);
            mail.IsBodyHtml = true;
            mail.Subject = "HwInf";
            mail.SubjectEncoding = Encoding.UTF8;


            mail.BodyEncoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        }

        public void MessageFormat(string status)
        {
            switch (status)
            {
                case "akzeptiert": AcceptMessage(_order); break;
                case "abgelehnt": DeclineMessage(_order); break;
                case "offen": NewOrderMessage(_order); break;
                default: mail.Body = status; break;
            }
        }

        public void AcceptMessage(Order order)
        {
            mail.Body += _bl.GetSetting("accept_mail_above").Value;
            mail.Body += "<br >";
            foreach (OrderItem ord in order.OrderItems)
            {
                if (ord.IsDeclined == false)
                {
                    mail.Body += ord.Device.Name + " : akzeptiert <br>";
                }
                else
                {
                    mail.Body += ord.Device.Name + " : abgelehnt <br>";
                }

            }
            mail.Body += _bl.GetSetting("accept_mail_below").Value;
        }

        public void DeclineMessage(Order order)
        {
            mail.Body += _bl.GetSetting("decline_mail_above").Value;
            mail.Body += "<br />";

            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body += ord.Device.Name + " : abgelehnt <br />";

            }

            mail.Body += _bl.GetSetting("decline_mail_below").Value;
        }
        public void NewOrderMessage(Order order)
        {
            mail.To.Clear();
            mail.To.Add(order.Verwalter.Email);
            mail.Body += _bl.GetSetting("new_order_mail").Value + "<br>";
            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body += "Name : " + ord.Device.Name + " | InventarNummer: " + ord.Device.InvNum + "<br />";
            }
        }

        public void ReminderMessage(Guid orderGuid)
        {

            Order order = _bl.GetOrders(orderGuid);
            mail.Body += _bl.GetSetting("reminder_mail").Value + "<br>";
            mail.Body += "Überfallig am: " + order.ReturnDate.ToShortDateString() + "<br>";

            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body += ord.Device.Name + "<br>";
            }
        }


        public void Send()
        {
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                _log.InfoFormat("Fehler beim senden" + ex);
            }

        }
    }
}
