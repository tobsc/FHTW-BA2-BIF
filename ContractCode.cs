using HwInf.Common.DAL;
using System.Diagnostics;
using System.Linq;

namespace HwInf.Common
{
    partial class Contract
    {
        //private int orderId;
        private string uid;
        public Contract(int orderId, string uid)
        {
            this.uid = uid;
            //this.orderId = orderId;
        }

        //returns part of contract where borrower is stated
        public string getEntleiher()
        {
           
            HwInfContext db = new HwInfContext();
            var p = db.Persons.Single(i => i.uid == this.uid);
            

            string entleiherPart="\\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Name: " + p.Name + " " + p.LastName + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Adresse: __________________________________ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Personenkennzahl: " + p.PersId + "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Studiengang: _______ } \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Telefon: " + p.Tel+ "} \\paragraph [ Format {SpaceAfter = \"1cm\" LeftIndent = \"2cm\"} ] {Email: " + p.Email+"}";
           
            return entleiherPart;
        }
    }
}
