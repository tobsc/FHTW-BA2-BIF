using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HwInf.Common;
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

            if(String.IsNullOrWhiteSpace(obj.PersonalType))
            {
                target.Role = "User";
            } else
            {
                target.Role = obj.PersonalType;
            }
            

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

        public static implicit operator Person(UserViewModel vmdl)
        {
            return new Person
            {
                PersId = vmdl.PersId,
                Name = vmdl.Name,
                LastName = vmdl.LastName,
                uid = vmdl.Uid,
                Email = vmdl.Email,
                Tel = vmdl.Tel,
                Room = vmdl.Room
            };
        }

        public static implicit operator UserViewModel(Person p)
        {
            return new UserViewModel
            {
                PersId = p.PersId,
                Name = p.Name,
                LastName = p.LastName,
                Uid = p.uid,
                Email = p.Email,
                Tel = p.Tel,
                Role = p.Role.Name,
                Room = p.Room
            };
        }
    }
}