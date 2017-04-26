using HwInf.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HwInf.Common
{
    partial class ReturnContract
    {
        private Order o;
        public ReturnContract(Order order)
        {
            this.o = order;
        }

        public string generate()
        {
            string text = "";
            foreach(OrderItem oi in o.OrderItems)
            {
                text += "\\section{\\paragraph[Format { Font { Bold = true} SpaceBefore = \"1cm\" SpaceAfter = \"0.25cm\"}]{Bestätigung der VerleiherIn über die Rückgabe des Gerätes:}\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Geräts der Marke " + oi.Device.Brand + " }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: " + oi.Device.Type.Name + "}\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: " + oi.Device.InvNum + "}\\paragraph[Format { SpaceAfter = \"0.5cm\"}]{Das oben genannte Gerät wurde heute}";

                //CHECK IF DAMAGED TODAY
                if (true)
                {
                    text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{O  in einwandfreiem Zustand und mit komplettem Zubehör (siehe Bestandsliste in der Anlage) zurückgegeben}\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{O  mit folgenden Mängeln/ Schäden zurückgegeben}\\paragraph[Format { SpaceAfter = \"0.25cm\" LeftIndent = \"1cm\"}]{...................................................}\\paragraph[Format { SpaceAfter = \"0.25cm\" LeftIndent = \"1cm\"}]{...................................................}\\paragraph[Format { SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{...................................................}\\paragraph[Format { SpaceAfter = \"1cm\"} ]{Wien, den "+DateTime.Now.Day.ToString("d2")+"."+DateTime.Now.Month.ToString("d2")+"."+DateTime.Now.Year.ToString()+"}\\paragraph[Format { SpaceAfter = \"1cm\"}{Unterschrift MitarbeiterIn des Instituts für Informatik: ......................................................................}}";
                }
            }

            return text;
        }
    }
}
