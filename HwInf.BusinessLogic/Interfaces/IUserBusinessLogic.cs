using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IUserBusinessLogic
    {
        IEnumerable<Person> GetUsers();
        Person GetUsers(string uid);
        Role GetRole(string name);
        Person CreateUser();
        void UpdateUser(Person obj);
        void SetAdmin(Person obj);
    }
}