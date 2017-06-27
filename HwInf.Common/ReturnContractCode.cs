using HwInf.Common.DAL;
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
        private readonly BL.BL _bl;

        public ReturnContract(Order order, BL.BL bl)
        {
            this.o = order;
            _bl = bl;
        }

        public string generate()
        {
            string text = "";
            var damages = _bl.GetDamages().ToList();

            foreach (var oi in o.OrderItems)
            {
                if (oi.IsDeclined == false)
                {
                    text += "\\section{\\paragraph[Format { Font { Bold = true} SpaceBefore = \"1cm\" SpaceAfter = \"0.25cm\"}]{Bestätigung der VerleiherIn über die Rückgabe des Gerätes:}\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Geräts der Marke " + oi.Device.Brand + " }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: " + oi.Device.Type.Name + "}\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: " + oi.Device.InvNum + "}\\paragraph[Format { SpaceAfter = \"0.5cm\"}]{Das oben genannte Gerät wurde heute}";

                    //sollte alle schäden zurückgeben, die vom oi sind und derzeit gemeldet sind
                    var damagesOfItem = damages.Where(i => i.Device.InvNum == oi.Device.InvNum)
                                                .Where(i => i.Date >= o.From)
                                                .Where(i => i.Date <= o.To)
                                                .Where(i => i.Cause.Uid == o.Entleiher.Uid);
                    if (damagesOfItem.Any())
                    {
                        text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{ mit folgenden Mängeln/ Schäden zurückgegeben}";

                        damagesOfItem.ToList().ForEach(i => text += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{ - " + i.Description + "}");
                    }
                    else
                    {
                        text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{O  in einwandfreiem Zustand und mit komplettem Zubehör zurückgegeben}";
                    }
                    text += "\\paragraph[Format {SpaceAfter = \"1cm\"} ]{Wien, den " + DateTime.Now.Day.ToString("d2") + "." + DateTime.Now.Month.ToString("d2") + "." + DateTime.Now.Year.ToString() + "}\\paragraph[Format { SpaceAfter = \"1cm\"}{Unterschrift MitarbeiterIn des Instituts für Informatik: ......................................................................}";
                    text += "\\paragraph{_____________________________                    _____________________________}\\paragraph{Unterschrift VerleiherIn                                     Unterschrift EntleiherIn}}";
                }
            }

            return text;
        }
    }
}
