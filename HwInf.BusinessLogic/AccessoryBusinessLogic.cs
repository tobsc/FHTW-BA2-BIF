using System.Collections.Generic;
using System.Linq;
using System.Security;
using HwInf.BusinessLogic.Interfaces;
using HwInf.Common.Models;
using HwInf.DataAccess.Interfaces;

namespace HwInf.BusinessLogic
{
    public class AccessoryBusinessLogic : IAccessoryBusinessLogic
    {
        private readonly IDataAccessLayer _dal;
        private readonly IBusinessLogicPrincipal _principal;

        public AccessoryBusinessLogic(IDataAccessLayer dal, IBusinessLogicPrincipal principal)
        {
            _dal = dal;
            _principal = principal;
        }
        public IEnumerable<Accessory> GetAccessories()
        {
            return _dal.Accessories;
        }

        public Accessory GetAccessory(string slug)
        {
            return _dal.Accessories.SingleOrDefault(i => slug.Equals(i.Slug));
        }
        public Accessory GetAccessory(int id)
        {
            return _dal.Accessories.SingleOrDefault(i => id.Equals(i.AccessoryId));
        }

        public Accessory CreateAccessory()
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            return _dal.CreateAccessory();
        }

        public void DeleteAccessory(Accessory a)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            _dal.DeleteAccessory(a);
        }

        public void UpdateAccessory(Accessory a)
        {
            if (!_principal.IsAllowed) throw new SecurityException();
            _dal.UpdateObject(a);
        }
    }
}