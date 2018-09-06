using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public UserBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }

        public IEnumerable<Person> GetUsers()
        {
            return _dal.Persons;
        }

        public Person GetUsers(string uid)
        {
            return _dal.Persons.SingleOrDefault(i => i.Uid == uid);
        }

        public Role GetRole(string name)
        {
            return _dal.Roles.SingleOrDefault(i => i.Name.Equals(name));
        }

        public Person CreateUser()
        {
            return _dal.CreatePerson();
        }

        public void UpdateUser(Person obj)
        {
            _dal.UpdateObject(obj);
        }

        public void SetAdmin(Person obj)
        {
            if (!_principal.IsAdmin) throw new SecurityException();
            obj.Role = GetRole("Admin");
        }
    }
}