using HwInf.Common.DAL;
using HwInf.Common.Models;
using System.Diagnostics;
using System.Linq;

namespace HwInf.Common
{
    partial class Contract
    {
        private Order o;
        public Contract(Order order)
        {
            o = order;
        }

        //returns part of contract where borrower is stated
        public string getEntleiher()
        {

            HwInfContext db = new HwInfContext();
            var p = o.Entleiher;


            string entleiherPart = "\\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Name: " + p.Name + " " + p.LastName + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Adresse: __________________________________ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Personenkennzahl: " + p.Uid + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Studiengang: _______ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Telefon: " + p.Tel + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Email: " + p.Email + "}";

            return entleiherPart;
        }

        public string getGeraete()
        {

            string geraete = "";

            foreach (var oi in o.OrderItems)
            {
                if (oi.IsDeclined == false)
                {
                    geraete += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Geräts der Marke " + oi.Device.Brand + " }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ: " + oi.Device.Type.Name + "}\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: " + oi.Device.InvNum + "} \\paragraph[Format { Font { Size = 7.5} SpaceAfter = \"0.25cm\"} ] {mit Zubehör an den/ die EntleiherIn für die befristete Nutzung auch außerhalb der Zeiten der Lehrveranstaltungen nach § 6 Absatz 1 dieses Leihvertrages}";
                }
            }

            return geraete;
        }
        public string getZeitspanne()
        {


            string zeitspannePart = "(1) Das Gerät wird für die Zeit vom " + o.From.ToString("dd.MM/yyyy") + " bis " + o.To.Date.ToString("dd.MM.yyyy") + " entliehen.";
            return zeitspannePart;
        }
        public string getAnhang()
        {
            var p = o.Entleiher;


            string anhangPart = "\\paragraph [ Format { SpaceAfter = \"1cm\"}]{zum Leihvertrag zwischen der FH Technikum Wien und " + p.Name + " " + p.LastName + "}\\paragraph  [ Format { SpaceAfter = \"1cm\"}]{Bestandsliste des Zubehörs für das}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Gerät der Marke DAS MUSS NOCH GENERIERT WERDEN!!!!}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Typ}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Inventarnummer:}";
            return anhangPart;
        }
    }
}
