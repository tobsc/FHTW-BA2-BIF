using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HwInf.Common;
using HwInf.Common.BL;
using HwInf.Common.DAL;
using HwInf.Common.Models;

namespace HwInf.ViewModels
{
    public class UserViewModel
    {

        public UserViewModel() { }

        public UserViewModel(Person obj)
        {
            Refresh(obj);
        }

        private int PersId { get; set; }
        public string Uid { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        private string Role { get; set; }
        public string Room { get; set; }
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
            target.Uid = source.Uid;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.Role = source.Role.Name;
            target.Room = source.Room;
            target.Password = null;
        }

        public void Refresh(LDAPUserParameters obj)
        {

            if (obj == null)
            {
                return;
            }

            var target = this;
            var source = obj;

            target.Name = source.Firstname;
            target.LastName = source.Lastname;
            target.Email = source.Mail;

            switch (source.PersonalType)
            {
                case "Teacher":
                    target.Role = "Owner";
                    break;
                default:
                    target.Role = "User";
                    break;
            }
        

        }

        public void ApplyChanges(Person obj, BL bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.LastName = source.LastName;
            target.Uid = source.Uid;
            target.Email = source.Email;
            target.Tel = source.Tel;
            target.Role = bl.GetRole(source.Role);
            target.Room = source.Room;
            

        }
        public void ApplyChangesTelRoom(Person obj)
        {
            var target = obj;
            var source = this;

            target.Tel = source.Tel;
            target.Room = source.Room;

        }
    }
}