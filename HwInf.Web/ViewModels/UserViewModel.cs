using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.LDAP;
using HwInf.Common.Models;

namespace HwInf.Web.ViewModels
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
        public string Role { get; set; }
        public string Room { get; set; }
        public string Password { get; set; }
        public string Studiengang { get; set; }

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
            target.Studiengang = source.Studiengang;
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
            target.Studiengang = source.StudiengangKuerzel;

            switch (source.PersonalType)
            {
                case "Teacher":
                    target.Role = "Verwalter";
                    break;
                default:
                    target.Role = "User";
                    break;
            }
        

        }

        public void ApplyChanges(Person obj, IBusinessLogicFacade bl)
        {
            var target = obj;
            var source = this;

            target.Name = source.Name;
            target.LastName = source.LastName;
            target.Uid = source.Uid;
            target.Email = source.Email;
            if (target.Tel == null)
            {
                target.Tel = source.Tel;
            }
            if (target.Role == null)
            {
                target.Role = bl.GetRole(source.Role);
            }
            target.Room = source.Room;
            if (target.Studiengang == null)
            {
                target.Studiengang = source.Studiengang;
            }
            

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