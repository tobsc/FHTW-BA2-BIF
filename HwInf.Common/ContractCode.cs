using HwInf.Common.DAL;
using System.Linq;

namespace HwInf.Common
{
    partial class Contract
    {
        //private int orderId;
        private readonly string _uid;
        public Contract(int orderId, string uid)
        {
            this._uid = uid;
            //this.orderId = orderId;
        }

        //returns part of contract where borrower is stated
        public string GetEntleiher()
        {
           
            var db = new HwInfContext();
            var p = db.Persons.Single(i => i.uid == _uid);
            

            var entleiherPart="Name: " + p.Name + " " + p.LastName + "\nAdresse: __________________________________ \nPersonenkennzahl: "+ p.PersId + "\nStudiengang: _______ \nTelefon: "+p.Tel+"\nEmail: "+p.Email+"\n";
            return entleiherPart;
        }
    }
}
