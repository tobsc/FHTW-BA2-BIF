using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.Services.Config;
using Microsoft.Extensions.Logging;

namespace HwInf.Services.MailService
{
    public class Mail : IMailService
    {
        private SmtpClient smtpClient;
        private MailMessage mail;
        private readonly  IBusinessLogicFacade _bl;
        private readonly ILogger<Mail> _log;
        private Order _order;

        public Mail(Guid orderGuid, IBusinessLogicFacade bl, ILogger<Mail> log = null)
        {
            _bl = bl;
            _log = log ?? new LoggerFactory().CreateLogger<Mail>();

            var un = MailConfig.Current.UserName;   
            var pw = MailConfig.Current.Password;
            var ms = MailConfig.Current.SmtpServer;
            var port = MailConfig.Current.Port;

            var cred = new NetworkCredential(un, pw);
            smtpClient = new SmtpClient(ms, port)
            {
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false,
                Credentials = cred
            };

            _order = _bl.GetOrders(orderGuid);
            string to = _order.Entleiher.Email;
            string from = MailConstants.FromMailAddress;


            mail = new MailMessage(from, to);
            mail.IsBodyHtml = true;
            mail.Subject = MailConstants.Subject;
            mail.SubjectEncoding = Encoding.UTF8;


            mail.BodyEncoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        }

        public void AddContactMessage()
        {
            mail.Body += string.Format(MailConstants.ContactMessage, _order.Verwalter.Email);
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
                    mail.Body +=  string.Format(MailConstants.AcceptedMessage, ord.Device.Name);
                }
                else
                {
                    mail.Body += string.Format(MailConstants.DeclinedMessage, ord.Device.Name);
                }

            }
            AddContactMessage();
            mail.Body += _bl.GetSetting("accept_mail_below").Value;
        }

        public void DeclineMessage(Order order)
        {
            mail.Body += _bl.GetSetting("decline_mail_above").Value;
            mail.Body += "<br />";

            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body += string.Format(MailConstants.DeclinedMessage, ord.Device.Name);

            }
            AddContactMessage();
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
            mail.Body += "Überfallig am: " + order.To.ToShortDateString() + "<br>";

            foreach (OrderItem ord in order.OrderItems)
            {
                mail.Body += ord.Device.Name + "<br>";
            }

            AddContactMessage();

        }


        public void Send()
        {
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                _log.LogWarning("Fehler beim senden" + ex);
            }

        }
    }
}
