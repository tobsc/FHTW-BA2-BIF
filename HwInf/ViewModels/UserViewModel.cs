using HwInf.Common.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HwInf.ViewModels
{
    public class UserViewModel
    {

        public UserViewModel(Person obj)
        {
            Refresh(obj);
        }

        public int PersId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Role { get; set; }
        public string Room { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public void Refresh(Person obj)
        {

            if(obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.PersId = source.PersId;
            target.Name = source.Name;
            target.LastName = source.LastName;
            target.Uid = source.uid;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.Role = source.Role.Name;
            target.Room = source.Room;
            
        }

        public void ApplyChanges(Person obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.LastName = source.LastName;
            target.uid = source.Uid;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.Role = db.Roles.Single(i => i.Name == source.Role);
            target.Room = source.Room;
            

        }
        public void ApplyChangesToTel(Person obj, HwInfContext db)
        {
            var target = obj;
            var source = this;

            target.Tel = source.Tel;

        }
    }
}