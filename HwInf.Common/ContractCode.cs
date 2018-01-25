using HwInf.Common.DAL;
using HwInf.Common.Models;
using System;
using System.Diagnostics;
using System.Linq;
using HwInf.Common.Interfaces;

namespace HwInf.Common
{
    partial class Contract
    {
        private Order o;
        private readonly IBusinessLayer _bl;

        public Contract(Order order, IBusinessLayer bl)
        {
            o = order;
            _bl = bl;
        }

        //returns part of contract where borrower is stated
        public string getEntleiher()
        {

            var p = o.Entleiher;


            string entleiherPart = "\\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Name: " 
                + p.Name 
                + " " 
                + p.LastName 
                + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Adresse: } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Personenkennzahl: " 
                + p.Uid 
                + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Studiengang: } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Telefon: " 
                + p.Tel 
                + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Email: " 
                + p.Email 
                + "}";

            return entleiherPart;
        }

        public string getGeraete()
        {

            string geraete = "";

            geraete += "\\paragraph[Format {Font { Size = 7.5}SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"} ]{(1) Gegenstand des Vertrages ist die Überlassung von:}";

            foreach (var oi in o.OrderItems)
            {
                
                if (oi.IsDeclined == false)
                {
                    if (!String.IsNullOrWhiteSpace(oi.Device.Brand))
                    {
                        geraete += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Marke: " + oi.Device.Brand + "}";
                    }
                    geraete += "\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Name: "
                        + oi.Device.Name
                        + " }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: "
                        + oi.Device.Type.Name + "}";

                        if (!String.IsNullOrWhiteSpace(oi.Device.InvNum))
                    {
                        geraete += "\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: "
                        + oi.Device.InvNum + "}";
                    }

                    if (!string.IsNullOrWhiteSpace(oi.Accessories))
                    {
                        geraete += "\\paragraph[Format { SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Zubehör: "
                            + oi.Accessories 
                            +"} ";
                    }
                 }
            }

            return geraete;
        }
        public string getZeitspanne()
        {


            string zeitspannePart = "(1) Das Gerät wird für die Zeit vom " + o.From.ToString("dd.MM.yyyy") + " bis " + o.To.Date.ToString("dd.MM.yyyy") + " entliehen.";
            return zeitspannePart;
        }

        public string getHandoff()
        {
            string text = "";
            var damages = _bl.GetDamages().ToList();

            foreach (var oi in o.OrderItems)
            {
                if (oi.IsDeclined == false)
                {
                    text += "\\section{\\paragraph[Format { Font { Bold = true} SpaceBefore = \"1cm\" SpaceAfter = \"0.25cm\"}]{Bestätigung der VerleiherIn über die Übernahme des Gerätes:}";

                    if (!String.IsNullOrWhiteSpace(oi.Device.Brand))
                    {
                        text += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Marke: " + oi.Device.Brand +"}";
                    }
                    text += "\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Name: "
                   + oi.Device.Name
                   + " }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: "
                   + oi.Device.Type.Name + "}";


                    if (!String.IsNullOrWhiteSpace(oi.Device.InvNum) )
                    {
                        text += "\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: "
                        + oi.Device.InvNum + "}";
                    }
                        
                        text += "\\paragraph[Format { SpaceAfter = \"0.5cm\"}]{Das oben genannte Gerät wurde heute}";

                    //sollte alle schäden zurückgeben, die vom oi sind und derzeit gemeldet sind
                    var damagesOfItem = damages.Where(i => i.Device == null || i.Device.DeviceId == oi.Device.DeviceId)
                                                .Where(i => i.Date <= DateTime.Now)
                                                .Where(i => i.DamageStatus == null || i.DamageStatus.Slug == "gemeldet")
                                                .ToList();
                    if (damagesOfItem.Any())
                    {
                        text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{ mit folgenden Mängeln/ Schäden übernommen}";

                        damagesOfItem.ToList().ForEach(i => text += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{ - " 
                        + i.Description 
                        + "}");
                    }
                    else
                    {
                        text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{O  in einwandfreiem Zustand und mit komplettem Zubehör übernommen}";
                    }
                    text += "\\paragraph[Format {SpaceAfter = \"1cm\"} ]{Wien, den "
                        + DateTime.Now.Day.ToString("d2")
                        + "."
                        + DateTime.Now.Month.ToString("d2")
                        + "."
                        + DateTime.Now.Year.ToString()
                        + "}"; //\\paragraph[Format { SpaceAfter = \"1cm\"}]{Unterschrift MitarbeiterIn des Instituts für Informatik: ................................................}";
                    text += "\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{_____________________________                    _____________________________}\\paragraph[Format { SpaceAfter = \"0.25cm\"}]{Unterschrift VerleiherIn                                     Unterschrift EntleiherIn}}";
                }
            }

            return text;
        }
    }
}
