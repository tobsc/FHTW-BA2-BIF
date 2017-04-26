using HwInf.Common.DAL;
using HwInf.Common.Models;
using System.Diagnostics;
using System.Linq;

namespace HwInf.Common
{
    partial class Contract
    {
        private string uid;
        private int orderId;
        public Contract(int orderId, string uid)
        {
            this.uid = uid;
            this.orderId = orderId;
        }

        //returns part of contract where borrower is stated
        public string getEntleiher()
        {

            HwInfContext db = new HwInfContext();
            var p = db.Persons.Single(i => i.Uid == this.uid);


            string entleiherPart = "\\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Name: " + p.Name + " " + p.LastName + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Adresse: __________________________________ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Personenkennzahl: " + p.Uid + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Studiengang: _______ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Telefon: " + p.Tel + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Email: " + p.Email + "}";

            return entleiherPart;
        }

        public string getGeraete()
        {
            HwInfContext db = new HwInfContext();
            var o = db.Orders.Single(i => i.OrderId == this.orderId);

            string geraete = "";

            foreach (OrderItem oi in o.OrderItems)
            {
                geraete += "\\paragraph[Format {SpaceAfter = \"0.5cm\" LeftIndent = \"1cm\"}]{Geräts der Marke "+oi.Device.Brand+" }\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{ Typ}\\paragraph[Format {SpaceAfter = \"0.5cm\"LeftIndent = \"1cm\"}]{Inventarnummer: "+oi.Device.InvNum+"} \\paragraph[Format { Font { Size = 7.5} SpaceAfter = \"0.25cm\"} ] {mit Zubehör an den/ die EntleiherIn für die befristete Nutzung auch außerhalb der Zeiten der Lehrveranstaltungen nach § 6 Absatz 1 dieses Leihvertrages}";

            }
            geraete = "\\paragraph{HELLO ASDFSFADAF}";

            return geraete;
        }
        public string getZeitspanne()
        {
            HwInfContext db = new HwInfContext();
            var o = db.Orders.Single(i => i.OrderId == this.orderId);



            string zeitspannePart = "(1) Das Gerät wird für die Zeit vom " + o.From.ToString("dd.MM/yyyy") + " bis " + o.To.Date.ToString("dd.MM.yyyy") + " entliehen.";
            return zeitspannePart;
        }
        public string getAnhang()
        {
            HwInfContext db = new HwInfContext();
            var p = db.Persons.Single(i => i.Uid == this.uid);


            string anhangPart = "\\paragraph [ Format { SpaceAfter = \"1cm\"}]{zum Leihvertrag zwischen der FH Technikum Wien und " + p.Name + " " + p.LastName + "}\\paragraph  [ Format { SpaceAfter = \"1cm\"}]{Bestandsliste des Zubehörs für das}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Gerät der Marke DAS MUSS NOCH GENERIERT WERDEN!!!!}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Typ}\\paragraph [ Format { SpaceAfter = \"1cm\" LeftIndent = \"1cm\"}]{Inventarnummer:}";
            return anhangPart;
        }
    }
}
