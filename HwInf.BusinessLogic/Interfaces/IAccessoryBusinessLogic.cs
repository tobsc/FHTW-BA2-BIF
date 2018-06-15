using System.Collections.Generic;
using HwInf.Common.Models;

namespace HwInf.BusinessLogic.Interfaces
{
    public interface IAccessoryBusinessLogic
    {
        IEnumerable<Accessory> GetAccessories();
        Accessory GetAccessory(string slug);
        Accessory GetAccessory(int id);
        Accessory CreateAccessory();
        void DeleteAccessory(Accessory a);
        void UpdateAccessory(Accessory a);
    }
}