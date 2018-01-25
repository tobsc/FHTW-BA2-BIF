using HwInf.Common.DAL;
using HwInf.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HwInf.Common.Interfaces;

namespace HwInf.Common
{
    partial class ReturnContract
    {
        private Order o;
        private readonly IBusinessLayer _bl;

        public ReturnContract(Order order, IBusinessLayer bl)
        {
            o = order;
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
                    text += "\\section{\\paragraph[Format { Font { Bold = true} SpaceBefore = \"1cm\" SpaceAfter = \"0.25cm\"}]{Bestätigung der VerleiherIn über die Rückgabe des Gerätes:}\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Geräts mit dem Namen "
                        + oi.Device.Name + "}";

                    if (!String.IsNullOrWhiteSpace(oi.Device.Brand))
                    {
                        text += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Marke: " 
                            + oi.Device.Brand + "}";
                    }
                    text += " \\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: "
                        + oi.Device.Type.Name + "}";
                        if (!String.IsNullOrWhiteSpace(oi.Device.InvNum))
                    {
                        text += "\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: "
                        + oi.Device.InvNum + "}";
                    }
                        text += "\\paragraph[Format { SpaceAfter = \"0.5cm\"}]{Das oben genannte Gerät wurde heute}";

                    //sollte alle schäden zurückgeben, die vom oi sind und derzeit gemeldet sind
                    var damagesOfItem = damages.Where(i => i.Device.InvNum == oi.Device.InvNum)
                                                .Where(i => i.Date.Date >= o.Date.Date)
                                                .Where(i => i.Cause.Uid == o.Entleiher.Uid)
                                                .Where(i => i.DamageStatus.Slug == "gemeldet");
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
