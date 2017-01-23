using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common
{
    public class Mail
    {
        private SmtpClient smtpClient;
        private MailMessage mail;

        public Mail()
        {
            smtpClient = new SmtpClient("localhost", 25);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;


            mail = new MailMessage();
            mail.Subject = "HwInf";
            mail.SubjectEncoding = Encoding.UTF8;
            mail.From = new MailAddress("hwinf@technikum-wien.at");
            mail.BodyEncoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        }


        public void To(string to)
        {
            mail.To.Add(new MailAddress(to));
        }

        public void Message(string status)
        {

            if(status == "accept")
            {
                mail.Body = "Ihre Anfrage wurde akzeptiert.";
            }

            else if(status == "decline")
            {
                mail.Body = "Ihre Anfrage wurde abgelehnt. Für Details besuchen Sie bitte <LinkZurSeite>";
            }

            else if(status == "newOrder")
            {
                mail.Body = "Es wurde eine neue Anfrge für eines Ihrer Geräte gestellt.";
            } else
            {
                mail.Body = status;
            }
            
        }


        public void Send()
        {
            smtpClient.Send(mail);
        }
    }
}
